using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserVerificationRepository
    {
        Task<UserVerification?> GetByIdAsync(Ulid id);
        Task<UserVerification?> GetByUserIdAsync(Ulid userId);
        Task<UserVerification?> GetByUserIdAndCodeAsync(Ulid userId, string code);
        Task AddAsync(UserVerification verification);
        Task MarkAsUsedAsync(Ulid id);
    }
}
