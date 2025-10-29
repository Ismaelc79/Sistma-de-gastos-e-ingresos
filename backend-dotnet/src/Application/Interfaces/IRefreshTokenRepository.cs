using Domain.Entities;

namespace Application.Interfaces;

public interface IRefreshTokenRepository : IRepository<RefreshToken, string>
{
    Task<RefreshToken?> GetByIdAsync(string id);
    Task<IEnumerable<RefreshToken>> GetByUserIdAsync(string userId);
    Task<RefreshToken?> GetByJwtIdAsync(string jwtId);
    Task<bool> IsTokenRevokedAsync(string id);
    Task RevokeTokenAsync(string id);
    Task RevokeAllUserTokensAsync(string userId);
}
