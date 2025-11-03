using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public CategoryRepository(IDbConnection connection, IDbTransaction transaction) 
        {
            _connection = connection;
            _transaction = transaction;
        }

        public async Task<Category> AddAsync(Category category)
        {
            const string sql = @"
                INSERT INTO [dbo].[Category] (UserId, Name, Type) 
                OUTPUT INSERTED.*
                VALUES (@UserId, @Name, @Type)
            ";

            return await _connection.QuerySingleAsync<Category>(
                sql,
                new { UserId = category.UserId, Name = category.Name, Type = category.Type}, 
                _transaction
            );
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = @"
                DELETE FROM [dbo].[Category]
                WHERE Id = @Id
            ";

            var rows = await _connection.ExecuteAsync(
                sql, 
                new { Id = id }, 
                _transaction
            );

            return rows > 0;
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            const string sql = @"
                SELECT * FROM [dbo].[Category] 
                WHERE ID = @Id
            ";

            return await _connection.QueryFirstOrDefaultAsync<Category>(
                sql,
                new { Id = id },
                _transaction
            );
        }

        public async Task<IEnumerable<Category>> GetByUserIdAsync(Ulid userId)
        {
            const string sql = @"
                SELECT * FROM [dbo].[Category]
                WHERE UserId = @UserId
            ";

            return await _connection.QueryAsync<Category>(
                sql,
                new { UserId = userId },
                _transaction
            );
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            const string sql = @"
                UPDATE [dbo].[Category]
                SET Name = @Name, Type = @Type
                OUTPUT INSERTED.*
                WHERE ID = @ID
            ";

            return await _connection.QuerySingleAsync<Category>(
                sql,
                new { ID = category.Id, Name = category.Name, Type = category.Type },
                _transaction
            );
        }

        public async Task<IEnumerable<Category>> GetByUserIdAndTypeAsync(Ulid userId, string type)
        {
            const string sql = @"
                SELECT * FROM [dbo].[Category]
                WHERE UserId = @UserId AND Type = @Type
            ";

            return await _connection.QueryAsync<Category>(
                sql,
                new { UserId = userId, Type = type },
                _transaction
            );
        }
    }
}
