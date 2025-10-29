using SistemaGastos.Domain.Entities;

namespace SistemaGastos.Domain.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByIdAsync(string id);
        Task<IEnumerable<RefreshToken>> GetByUserIdAsync(string userId);
        Task AddAsync(RefreshToken token);
        Task RevokeAsync(RefreshToken token);
    }
}
