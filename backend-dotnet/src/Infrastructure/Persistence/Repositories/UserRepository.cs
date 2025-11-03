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
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public UserRepository(IDbConnection connection, IDbTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public async Task<User> AddAsync(User user)
        {
            const string sql = @"
                INSERT INTO [dbo].[User] (ID, Name, Email, PasswordHash, Phone, Currency, Language, Avatar)
                OUTPUT INSERTED.*
                VALUES (@ID, @Name, @Email, @PasswordHash, @Phone, @Currency, @Language, @Avatar)
            ";

            return await _connection.QuerySingleAsync<User>(
                sql,
                new { ID = user.Id, Name = user.Name, Email = user.Email, PasswordHash = user.PasswordHash, Phone = user.Phone, Currency = user.Currency, Language = user.Language, Avatar = user.Avatar },
                _transaction
            );
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            const string sql = @"
                SELECT * FROM [dbo].[User]
                WHERE Email = @Email
            ";

            return await _connection.QueryFirstOrDefaultAsync<User>(
                sql,
                new { Email = email },
                _transaction
            );
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            const string sql = @"
                SELECT CASE WHEN EXISTS(
                    SELECT 1 FROM [dbo].[User]
                    WHERE Email = @Email
                )
                THEN 1 ELSE 0 END
            ";

            var result = await _connection.ExecuteScalarAsync<int>(
                sql,
                new { Email = email },
                _transaction
            );

            return result.Equals(1);
        }

        public async Task<User?> GetByIdAsync(Ulid id)
        {
            const string sql = @"
                SELECT * FROM [dbo].[User]
                WHERE ID = @ID
            ";

            return await _connection.QueryFirstOrDefaultAsync<User>(
                sql,
                new { ID = id },
                _transaction
            );
        }

        public async Task<User> UpdateAsync(User user)
        {
            const string sql = @"
                UPDATE FROM [dbo].[User]
                SET Name = @Name, Email = @Email, PasswordHash = @PasswordHash, Phone = @Phone, Currency = @Currency, Language = @Language, Avatar = @Avatar
                OUTPUT INSERTED.*
                WHERE ID = @ID
            ";

            return await _connection.QuerySingleAsync<User>(
                sql,
                new { ID = user.Id.ToString(), Name = user.Name, Email = user.Email, PasswordHash = user.PasswordHash, Phone = user.Phone, Currency = user.Currency, Language = user.Language, Avatar = user.Avatar},
                _transaction
            );
        }
    }
}
