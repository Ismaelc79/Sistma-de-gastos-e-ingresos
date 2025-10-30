using Application.DTOs.Auth;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using System.Security.Cryptography;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserDto?> GetUserByIdAsync(string id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        return user != null ? _mapper.Map<UserDto>(user) : null;
    }

    public async Task<UserDto?> GetUserByEmailAsync(string email)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(email);
        return user != null ? _mapper.Map<UserDto>(user) : null;
    }

    public async Task<UserDto> UpdateUserProfileAsync(string userId, UpdateProfileRequest request)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException($"Usuario con ID {userId} no encontrado");
        }

        if (!string.IsNullOrEmpty(request.Name))
            user.Name = request.Name;

        if (!string.IsNullOrEmpty(request.Phone))
            user.Phone = request.Phone;

        if (!string.IsNullOrEmpty(request.Currency))
            user.Currency = request.Currency;

        if (!string.IsNullOrEmpty(request.Language))
            user.Language = request.Language;

        if (!string.IsNullOrEmpty(request.Avatar))
            user.Avatar = request.Avatar;

        user.UpdatedAt = DateTime.UtcNow;

        var updatedUser = await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<UserDto>(updatedUser);
    }

    public async Task<bool> DeleteUserAsync(string userId)
    {
        return await _unitOfWork.Users.DeleteAsync(userId);
    }

    public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException($"Usuario con ID {userId} no encontrado");
        }

        if (!VerifyPassword(currentPassword, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Contraseña actual incorrecta");
        }

        user.PasswordHash = HashPassword(newPassword);
        user.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    // Helper methods
    private string HashPassword(string password)
    {
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
