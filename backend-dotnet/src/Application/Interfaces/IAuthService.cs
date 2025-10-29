using Application.DTOs.Auth;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request);
    Task<bool> LogoutAsync(string userId, string refreshTokenId);
    Task<UserDto?> GetCurrentUserAsync(string userId);
    Task<bool> VerifyEmailAsync(string userId, string code);
    Task<bool> SendVerificationCodeAsync(string email);
}
