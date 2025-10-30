using Domain.Interfaces;
using Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        private bool _completed;

        public UnitOfWork(DapperContext context)
        {
            _connection = context.CreateConnection();
            _transaction = _connection.BeginTransaction();
        }

        // Repository exhibition
        private ICategoryRepository? _categoryRepository;
        private IRefreshTokenRepository? _refreshTokenRepository;
        private ITransactionRepository? _transactionRepository;
        private IUserRepository? _userRepository;
        private IUserVerificationRepository? _userVerificationRepository;

        // Repository lazy loarding
        public ICategoryRepository Category => _categoryRepository ??= new CategoryRepository(_connection, _transaction);
        public IRefreshTokenRepository RefreshToken => _refreshTokenRepository ??= new RefreshTokenRepository(_connection, _transaction);
        public ITransactionRepository Transaction => _transactionRepository ??= new TransactionRepository(_connection, _transaction);
        public IUserRepository User => _userRepository ??= new UserRepository(_connection, _transaction);
        public IUserVerificationRepository UserVerification => _userVerificationRepository ??= new UserVerificationRepository(_connection, _transaction);

        // Manages transactions
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _transaction.Commit();
                _completed = true;
                _connection.Close();
                return Task.FromResult(1);
            }
            catch
            {
                _transaction.Rollback();
                _connection.Close();
                throw;
            }
        }

        // Space saving
        public void Dispose()
        {
            if (_completed is false)
            {
                try { _transaction.Rollback(); } catch { }
                try { _connection.Close(); } catch { }
            }

            _transaction.Dispose();
            _connection.Dispose();
        }
    }
}
