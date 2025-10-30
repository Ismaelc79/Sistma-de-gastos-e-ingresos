using Application.DTOs.Auth;
using Application.Interfaces;
using Domain.Entities;
using AutoMapper;
using System.Security.Cryptography;
using System.Text;

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
        if (await _unitOfWork.Users.EmailExistsAsync(request.Email))
        {
            throw new InvalidOperationException("El email ya está registrado");
        }

        // Crear nuevo usuario
        var user = new User
        {
            Id = GenerateUserId(), // Generar ID ULID o similar
            Email = request.Email,
            Name = request.Name,
            Phone = request.Phone,
            PasswordHash = HashPassword(request.Password),
            IsVerified = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Guardar usuario
        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        // Generar tokens
        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshTokenId = GenerateRefreshTokenId();
        var refreshTokenValue = _tokenService.GenerateRefreshToken();
        var jwtId = _tokenService.GetJwtIdFromToken(accessToken);

        // Guardar refresh token
        var refreshToken = new RefreshToken
        {
            Id = refreshTokenId,
            UserId = user.Id,
            JwtId = jwtId,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            Revoked = false,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.RefreshTokens.AddAsync(refreshToken);
        await _unitOfWork.SaveChangesAsync();

        // Crear código de verificación
        var verificationCode = GenerateVerificationCode();
        var userVerification = new UserVerification
        {
            Id = GenerateVerificationId(),
            UserId = user.Id,
            Code = verificationCode,
            ExpiresAt = DateTime.UtcNow.AddHours(24),
            Used = false,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.UserVerifications.AddAsync(userVerification);
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
        var user = await _unitOfWork.Users.GetByEmailAsync(request.Email);
        if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Email o contraseña incorrectos");
        }

        // Generar tokens
        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshTokenId = GenerateRefreshTokenId();
        var refreshTokenValue = _tokenService.GenerateRefreshToken();
        var jwtId = _tokenService.GetJwtIdFromToken(accessToken);

        // Guardar refresh token
        var refreshToken = new RefreshToken
        {
            Id = refreshTokenId,
            UserId = user.Id,
            JwtId = jwtId,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            Revoked = false,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.RefreshTokens.AddAsync(refreshToken);
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
        var refreshToken = await _unitOfWork.RefreshTokens.GetByJwtIdAsync(jwtId);

        if (refreshToken == null || refreshToken.Revoked || refreshToken.ExpiresAt < DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException("Refresh token expirado o revocado");
        }

        // Obtener usuario
        var userId = principal.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("Usuario no encontrado en token");
        }

        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Usuario no existe");
        }

        // Generar nuevo access token
        var newAccessToken = _tokenService.GenerateAccessToken(user);
        var newJwtId = _tokenService.GetJwtIdFromToken(newAccessToken);

        // Revocar token anterior
        await _unitOfWork.RefreshTokens.RevokeTokenAsync(refreshToken.Id);

        // Crear nuevo refresh token
        var newRefreshTokenId = GenerateRefreshTokenId();
        var newRefreshTokenValue = _tokenService.GenerateRefreshToken();
        var newRefreshToken = new RefreshToken
        {
            Id = newRefreshTokenId,
            UserId = user.Id,
            JwtId = newJwtId,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            Revoked = false,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.RefreshTokens.AddAsync(newRefreshToken);
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

    public async Task<bool> LogoutAsync(string userId, string refreshTokenId)
    {
        await _unitOfWork.RefreshTokens.RevokeTokenAsync(refreshTokenId);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<UserDto?> GetCurrentUserAsync(string userId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        return user != null ? _mapper.Map<UserDto>(user) : null;
    }

    public async Task<bool> VerifyEmailAsync(string userId, string code)
    {
        var verification = await _unitOfWork.UserVerifications.GetByUserIdAndCodeAsync(userId, code);

        if (verification == null || verification.Used || verification.ExpiresAt < DateTime.UtcNow)
        {
            return false;
        }

        // Marcar como usado
        await _unitOfWork.UserVerifications.MarkAsUsedAsync(verification.Id);

        // Actualizar usuario como verificado
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user != null)
        {
            user.IsVerified = true;
            user.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.Users.UpdateAsync(user);
        }

        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SendVerificationCodeAsync(string email)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(email);
        if (user == null)
        {
            return false;
        }

        var verificationCode = GenerateVerificationCode();
        var userVerification = new UserVerification
        {
            Id = GenerateVerificationId(),
            UserId = user.Id,
            Code = verificationCode,
            ExpiresAt = DateTime.UtcNow.AddHours(24),
            Used = false,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.UserVerifications.AddAsync(userVerification);
        await _unitOfWork.SaveChangesAsync();

        // TODO: Enviar email con el código de verificación

        return true;
    }

    // Helper methods
    private string GenerateUserId()
    {
        // Generar ID de 26 caracteres usando Guid + timestamp
        return Guid.NewGuid().ToString("N").Substring(0, 26).ToUpper();
    }

    private string GenerateRefreshTokenId()
    {
        return Guid.NewGuid().ToString("N").Substring(0, 26).ToUpper();
    }

    private string GenerateVerificationId()
    {
        return Guid.NewGuid().ToString("N").Substring(0, 26).ToUpper();
    }

    private string GenerateVerificationCode()
    {
        // Generar código de 10 dígitos
        var random = new Random();
        return random.Next(1000000000, int.MaxValue).ToString().Substring(0, 10);
    }

    private string HashPassword(string password)
    {
        // Implementar hashing con PBKDF2 (built-in en .NET)
        using (var rng = new Rfc2898DeriveBytes(password, 16, 10000, HashAlgorithmName.SHA256))
        {
            var salt = rng.Salt;
            var hash = rng.GetBytes(32);
            var hashBytes = new byte[48];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 32);
            return Convert.ToBase64String(hashBytes);
        }
    }

    private bool VerifyPassword(string password, string hash)
    {
        // Verificar hashing con PBKDF2
        var hashBytes = Convert.FromBase64String(hash);
        var salt = new byte[16];
        Array.Copy(hashBytes, 0, salt, 0, 16);

        using (var rng = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
        {
            var hash2 = rng.GetBytes(32);
            for (int i = 0; i < 32; i++)
            {
                if (hashBytes[i + 16] != hash2[i])
                    return false;
            }
        }
        return true;
    }
}
