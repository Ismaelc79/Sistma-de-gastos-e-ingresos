using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Category { get; }
        IRefreshTokenRepository RefreshToken { get; }
        ITransactionRepository Transaction { get; }
        IUserRepository User { get; }
        IUserVerificationRepository UserVerification { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
