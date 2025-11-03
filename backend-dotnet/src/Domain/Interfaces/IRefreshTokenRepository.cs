using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByIdAsync(Ulid id);
        Task<RefreshToken?> GetByJwtIdAsync(string jwtId);
        Task<IEnumerable<RefreshToken>> GetByUserIdAsync(Ulid userId);
        Task AddAsync(RefreshToken token);
        Task RevokeTokenAsync(Ulid id);
        Task DeleteExpiredTokensAsync();
    }
}
