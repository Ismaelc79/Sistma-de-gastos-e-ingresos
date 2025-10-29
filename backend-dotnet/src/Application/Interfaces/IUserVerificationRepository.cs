using Domain.Entities;

namespace Application.Interfaces;

public interface IUserVerificationRepository : IRepository<UserVerification, string>
{
    Task<UserVerification?> GetByUserIdAndCodeAsync(string userId, string code);
    Task<IEnumerable<UserVerification>> GetByUserIdAsync(string userId);
    Task<bool> IsCodeValidAsync(string userId, string code);
    Task MarkAsUsedAsync(string id);
}
