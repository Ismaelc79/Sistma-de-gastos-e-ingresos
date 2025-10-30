using Application.DTOs.Auth;

namespace Application.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetUserByIdAsync(string id);
    Task<UserDto?> GetUserByEmailAsync(string email);
    Task<UserDto> UpdateUserProfileAsync(string userId, UpdateProfileRequest request);
    Task<bool> DeleteUserAsync(string userId);
    Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
}
