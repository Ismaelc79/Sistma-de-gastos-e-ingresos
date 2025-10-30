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

        public async Task AddAsync(Category category)
        {
            const string sql = @"
                INSERT INTO dbo.Category (UserId, Name, Type) 
                VALUES (@UserId, @Name, @Type)
            ";

            await _connection.ExecuteAsync(
                sql,
                new { UserId = category.UserId, Name = category.Name, Type = category.Type}, 
                _transaction
            );
        }

        public async Task DeleteAsync(int id)
        {
            const string sql = @"
                DELETE FROM dbo.Category 
                WHERE Id = @Id
            ";

            await _connection.ExecuteAsync(
                sql, 
                new { Id = id }, 
                _transaction
            );
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            const string sql = @"
                SELECT * FROM dbo.Category 
                WHERE ID = @Id
            ";

            return await _connection.QueryFirstOrDefaultAsync<Category>(
                sql,
                new { Id = id },
                _transaction
            );
        }

        public Task<IEnumerable<Category>> GetByUserIdAsync(string userId)
        {
            const string sql = @"
                SELECT * FROM dbo.Category
                WHERE UserId = @UserId
            ";

            return _connection.QueryAsync<Category>(
                sql,
                new { UserId = userId },
                _transaction
            );
        }

        public Task UpdateAsync(Category category)
        {
            const string sql = @"
                UPDATE dbo.Category
                SET Name = @Name, Type = @Type
                WHERE ID = @ID
            ";

            return _connection.ExecuteAsync(
                sql,
                new { ID = category.Id, Name = category.Name, Type = category.Type },
                _transaction
            );
        }
    }
}
