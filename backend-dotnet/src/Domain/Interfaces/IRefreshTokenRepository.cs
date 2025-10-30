using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByIdAsync(Ulid id);
        Task<IEnumerable<RefreshToken>> GetByUserIdAsync(Ulid userId);
        Task AddAsync(RefreshToken token);
        Task RevokeAsync(RefreshToken token);
    }
}
