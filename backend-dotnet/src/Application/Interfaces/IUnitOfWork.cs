namespace Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    ICategoryRepository Categories { get; }
    ITransactionRepository Transactions { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    IUserVerificationRepository UserVerifications { get; }

    Task<int> SaveChangesAsync();
    Task<bool> BeginTransactionAsync();
    Task<bool> CommitTransactionAsync();
    Task<bool> RollbackTransactionAsync();
}
