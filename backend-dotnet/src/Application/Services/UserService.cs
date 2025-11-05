using Application.DTOs.Auth;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
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

    public async Task<UserDto?> GetUserByIdAsync(Ulid id)
    {
        var user = await _unitOfWork.User.GetByIdAsync(id) ?? throw new KeyNotFoundException("Usuario no encontrado");
        return user != null ? _mapper.Map<UserDto>(user) : null;
    }

    public async Task<UserDto?> GetUserByEmailAsync(string email)
    {
        var user = await _unitOfWork.User.GetByEmailAsync(email);
        return user != null ? _mapper.Map<UserDto>(user) : null;
    }

    public async Task<UserDto> UpdateUserProfileAsync(Ulid userId, UpdateProfileRequest request)
    {
        var user = await _unitOfWork.User.GetByIdAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException($"Usuario con ID {userId} no encontrado");
        }
        user.UpdateProfile(
            name: request.Name,
            language: request.Language,
            avatar: request.Avatar
        );

        if (!string.IsNullOrEmpty(request.Phone))
            user.UpdateProfile(phone: new PhoneNumber(request.Phone));

        if (!string.IsNullOrEmpty(request.Currency))
            user.UpdateProfile(currency: new Currency(request.Currency));

        if (!string.IsNullOrEmpty(request.Password) && !string.IsNullOrEmpty(request.NewPassword))
        {
            await ChangePasswordAsync(userId, request.Password, request.NewPassword);
        }

        var updatedUser = await _unitOfWork.User.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<UserDto>(updatedUser);
    }

    public async Task<bool> ChangePasswordAsync(Ulid userId, string currentPassword, string newPassword)
    {
        var user = await _unitOfWork.User.GetByIdAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException($"Usuario con ID {userId} no encontrado");
        }

        if (!user.PasswordHash.Verify(currentPassword))
        {
            throw new UnauthorizedAccessException("Contraseña actual incorrecta");
        }

        var newPasswordd = Password.CreateHashed(newPassword);

        user.ChangePassword(newPasswordd);

        await _unitOfWork.User.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
