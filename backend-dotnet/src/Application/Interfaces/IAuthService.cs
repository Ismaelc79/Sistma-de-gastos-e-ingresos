using Application.DTOs.Auth;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request);
    Task<bool> LogoutAsync(Ulid refreshTokenId);
    Task<UserDto?> GetCurrentUserAsync(Ulid userId);
    Task<bool> VerifyEmailAsync(Ulid userId, string code);
    Task<bool> SendVerificationCodeAsync(string email);
}
