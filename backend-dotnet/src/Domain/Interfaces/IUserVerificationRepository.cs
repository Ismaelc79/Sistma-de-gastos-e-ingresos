using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserVerificationRepository
    {
        Task<UserVerification?> GetByIdAsync(string id);
        Task<UserVerification?> GetByUserIdAsync(string userId);
        Task AddAsync(UserVerification verification);
        Task MarkUsedAsync(UserVerification verification);
    }
}
