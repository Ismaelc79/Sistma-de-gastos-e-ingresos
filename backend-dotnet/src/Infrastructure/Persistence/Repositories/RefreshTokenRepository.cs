using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Persistence.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public RefreshTokenRepository(IDbConnection connection, IDbTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public async Task AddAsync(RefreshToken token)
        {
            const string sql = @"
                INSERT INTO dbo.RefreshToken (ID, UserId, JwtId, ExpiresAt, Revoked) 
                VALUES (@ID, @UserId, @JwtId, @ExpiresAt, @Revoked)
            ";

            await _connection.ExecuteAsync(
                sql, 
                new { ID = token.Id.ToString(), UserId = token.UserId.ToString(), JwtId = token.JwtId, ExpiresAt = token.ExpiresAt, Revoked = token.Revoked }, 
                _transaction
            );
        }

        public async Task<RefreshToken?> GetByIdAsync(Ulid id)
        {
            const string sql = @"
                SELECT * FROM dbo.RefreshToken 
                WHERE ID = @ID
            ";

            return await _connection.QueryFirstOrDefaultAsync<RefreshToken>(
                sql, 
                new { ID = id.ToString() },
                _transaction
            );
        }

        public async Task<IEnumerable<RefreshToken>> GetByUserIdAsync(Ulid userId)
        {
            const string sql = @"
                SELECT * FROM dbo.RefreshToken
                WHERE UserId = @UserId
            ";

            return await _connection.QueryAsync<RefreshToken>(
                sql,
                new { UserId = userId.ToString() },
                _transaction
           );
        }

        public async Task RevokeAsync(RefreshToken token)
        {
            const string sql = @"
                UPDATE dbo.RefreshToken
                SET Revoked = 1
                WHERE ID = @ID
            ";

            await _connection.ExecuteAsync(
                sql,
                new { ID = token.Id.ToString() },
                _transaction
            );
        }
    }
}
