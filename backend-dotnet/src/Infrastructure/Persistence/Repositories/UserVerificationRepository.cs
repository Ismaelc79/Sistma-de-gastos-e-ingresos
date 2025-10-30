using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class UserVerificationRepository : IUserVerificationRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public UserVerificationRepository(IDbConnection connection, IDbTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public async Task AddAsync(UserVerification verification)
        {
            const string sql = @"
                INSERT INTO dbo.UserVerification (ID, UserId, Code, ExpiresAt, Used)
                VALUES (@ID, @UserId, @Code, @ExpiresAt, @Used)
            ";

            await _connection.ExecuteAsync(
                sql,
                new { ID = verification.Id, UserId = verification.UserId, Code = verification.Code, ExpiresAt = verification.ExpiresAt, Used = verification.Used },
                _transaction
            );
        }

        public async Task<UserVerification?> GetByIdAsync(string id)
        {
            const string sql = @"
                SELECT * FROM dbo.UserVerification
                WHERE ID = @ID
            ";

            return await _connection.QueryFirstOrDefaultAsync<UserVerification>(
                sql,
                new { ID = id },
                _transaction
            );
        }

        public async Task<UserVerification?> GetByUserIdAsync(string userId)
        {
            const string sql = @"
                SELECT * FROM dbo.UserVerification
                WHERE UserId = @UserId
            ";

            return await _connection.QueryFirstOrDefaultAsync<UserVerification>(
                sql,
                new { UserId = userId },
                _transaction
            );
        }

        public async Task MarkUsedAsync(UserVerification verification)
        {
            const string sql = @"
                UPDATE dbo.UserVerification
                SET Used = 1
                WHERE ID = @ID
            ";

            await _connection.ExecuteAsync(
                sql,
                new { ID = verification.Id },
                _transaction
            );
        }
    }
}
