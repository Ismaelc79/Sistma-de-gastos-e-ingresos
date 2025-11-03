using Application.DTOs.Auth;
using Application.Interfaces;
using Domain.Entities;
using AutoMapper;
using System.Security.Cryptography;
using System.Text;
using Domain.ValueObjects;
using Domain.Interfaces;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AuthService(IUnitOfWork unitOfWork, ITokenService tokenService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        // Validar que el email no exista
        if (await _unitOfWork.User.EmailExistsAsync(request.Email))
        {
            throw new InvalidOperationException("El email ya está registrado");
        }

        // Crear value objects para User
        var email = new Email(request.Email);
        var password = Password.CreateHashed(request.Password);
        var phone = string.IsNullOrWhiteSpace(request.Phone) ? null : new PhoneNumber(request.Phone);

        // Crear nuevo usuario
        var user = new User(
            id: Ulid.NewUlid(),
            name: request.Name,
            email: email,
            password: password,
            phone: phone
        );

        // Guardar usuario
        await _unitOfWork.User.AddAsync(user);

        // Generar tokens
        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshTokenValue = _tokenService.GenerateRefreshToken();
        var jwtId = _tokenService.GetJwtIdFromToken(accessToken);

        // Guardar refresh token
        var refreshToken = new RefreshToken(
            id: Ulid.NewUlid(),
            userId: user.Id,
            jwtId: jwtId,
            expiresAt: DateTime.UtcNow.AddDays(7)
        );

        await _unitOfWork.RefreshToken.AddAsync(refreshToken);

        // Crear código de verificación
        var userVerification = new UserVerification(
            id: Ulid.NewUlid(),
            userId: user.Id,
            code: GenerateVerificationCode(),
            expiresAt: DateTime.UtcNow.AddHours(24)
        );

        await _unitOfWork.UserVerification.AddAsync(userVerification);
        await _unitOfWork.SaveChangesAsync();

        // TODO: Enviar email de verificación con el código

        var userDto = _mapper.Map<UserDto>(user);
        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenValue,
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            User = userDto
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        // Buscar usuario por email
        var user = await _unitOfWork.User.GetByEmailAsync(request.Email);

        if (user == null || !user.PasswordHash.Verify(request.Password))
        {
            throw new UnauthorizedAccessException("Email o contraseña incorrectos");
        }

        // Revoca tokens anteriores
        var refreshTokens = await _unitOfWork.RefreshToken.GetByUserIdAsync(user.Id);

        foreach (var tokens in refreshTokens)
        {
            await _unitOfWork.RefreshToken.RevokeTokenAsync(tokens.Id);
        }

        // Generar tokens
        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshTokenValue = _tokenService.GenerateRefreshToken();
        var jwtId = _tokenService.GetJwtIdFromToken(accessToken);

        // Guardar refresh token
        var refreshToken = new RefreshToken(
            id: Ulid.NewUlid(),
            userId: user.Id,
            jwtId: jwtId,
            expiresAt: DateTime.UtcNow.AddDays(7)
        );

        await _unitOfWork.RefreshToken.AddAsync(refreshToken);
        await _unitOfWork.SaveChangesAsync();

        var userDto = _mapper.Map<UserDto>(user);
        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenValue,
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            User = userDto
        };
    }

    public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        // Validar refresh token
        var principal = _tokenService.GetPrincipalFromExpiredToken(request.RefreshToken);
        if (principal == null)
        {
            throw new UnauthorizedAccessException("Token inválido");
        }

        var jwtId = _tokenService.GetJwtIdFromToken(request.RefreshToken);
        var refreshToken = await _unitOfWork.RefreshToken.GetByJwtIdAsync(jwtId);

        if (refreshToken == null || refreshToken.Revoked || refreshToken.ExpiresAt < DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException("Refresh token expirado o revocado");
        }

        // Obtener usuario
        var userIdString = principal.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userIdString))
        {
            throw new UnauthorizedAccessException("Usuario no encontrado en token");
        }

        if (!Ulid.TryParse(userIdString, out var userId))
        {
            throw new UnauthorizedAccessException("Formato inválido del identificador de usuario");
        }

        // Busca el usuario desde la base de datos
        var user = await _unitOfWork.User.GetByIdAsync(userId);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Usuario no existe");
        }

        // Generar nuevo access token
        var newAccessToken = _tokenService.GenerateAccessToken(user);
        var newJwtId = _tokenService.GetJwtIdFromToken(newAccessToken);

        // Revocar token anterior
        await _unitOfWork.RefreshToken.RevokeTokenAsync(refreshToken.Id);

        // Crear nuevo refresh token
        var newRefreshTokenValue = _tokenService.GenerateRefreshToken();

        var newRefreshToken = new RefreshToken(
            id: Ulid.NewUlid(),
            userId: user.Id,
            jwtId: jwtId,
            expiresAt: DateTime.UtcNow.AddDays(7)
        );

        await _unitOfWork.RefreshToken.AddAsync(newRefreshToken);
        await _unitOfWork.SaveChangesAsync();

        var userDto = _mapper.Map<UserDto>(user);
        return new AuthResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshTokenValue,
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            User = userDto
        };
    }

    public async Task<bool> LogoutAsync(Ulid refreshTokenId)
    {
        await _unitOfWork.RefreshToken.RevokeTokenAsync(refreshTokenId);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<UserDto?> GetCurrentUserAsync(Ulid userId)
    {
        var user = await _unitOfWork.User.GetByIdAsync(userId);
        return user != null ? _mapper.Map<UserDto>(user) : null;
    }

    public async Task<bool> VerifyEmailAsync(Ulid userId, string code)
    {
        var verification = await _unitOfWork.UserVerification.GetByUserIdAndCodeAsync(userId, code);

        if (verification == null || verification.Used || verification.ExpiresAt < DateTime.UtcNow)
        {
            return false;
        }

        // Marcar como usado
        await _unitOfWork.UserVerification.MarkAsUsedAsync(verification.Id);

        // Actualizar usuario como verificado
        var user = await _unitOfWork.User.GetByIdAsync(userId);

        if (user != null)
        {
            user.Verify();
            await _unitOfWork.User.UpdateAsync(user);
        }

        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SendVerificationCodeAsync(string email)
    {
        var user = await _unitOfWork.User.GetByEmailAsync(email);
        if (user == null)
        {
            return false;
        }

        var verificationCode = GenerateVerificationCode();

        var userVerification = new UserVerification(
            id: Ulid.NewUlid(),
            userId: user.Id,
            code: verificationCode,
            expiresAt: DateTime.UtcNow.AddHours(24)
        );

        await _unitOfWork.UserVerification.AddAsync(userVerification);
        await _unitOfWork.SaveChangesAsync();

        // TODO: Enviar email con el código de verificación

        return true;
    }

    private string GenerateVerificationCode()
    {
        // Generar código de 10 dígitos
        var random = new Random();
        return random.Next(1000000000, int.MaxValue).ToString().Substring(0, 10);
    }
}
