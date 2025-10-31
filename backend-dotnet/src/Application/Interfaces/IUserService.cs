using Application.DTOs.Auth;

namespace Application.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetUserByIdAsync(Ulid id);
    Task<UserDto?> GetUserByEmailAsync(string email);
    Task<UserDto> UpdateUserProfileAsync(Ulid userId, UpdateProfileRequest request);
    Task<bool> ChangePasswordAsync(Ulid userId, string currentPassword, string newPassword);
}
