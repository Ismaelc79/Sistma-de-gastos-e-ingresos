using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserVerificationRepository
    {
        Task<UserVerification?> GetByIdAsync(Ulid id);
        Task<UserVerification?> GetByUserIdAsync(Ulid userId);
        Task AddAsync(UserVerification verification);
        Task MarkUsedAsync(UserVerification verification);
    }
}
