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
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public TransactionRepository(IDbConnection connection, IDbTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public async Task<Transaction> AddAsync(Transaction transaction)
        {
            const string sql = @"
                INSERT INTO [dbo].[Transaction] (CategoryId, UserId, Name, Description)
                OUTPUT INSERTED.*
                VALUES (@CategoryId, @UserId, @Name, @Description)
            ";

            return await _connection.QuerySingleAsync<Transaction>(
                sql,
                new { CategoryId = transaction.CategoryId, UserId = transaction.UserId.ToString(), Name = transaction.Name, Descripcion = transaction.Description },
                _transaction
            );
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = @"
                DELETE FROM [dbo].[Transaction]
                WHERE ID = @ID
            ";

            var rows = await _connection.ExecuteAsync(
                sql,
                new { ID = id },
                _transaction
            );

            return rows > 0;
        }

        public async Task<IEnumerable<Transaction>> GetByCategoryIdAsync(int categoryId)
        {
            const string sql = @"
                SELECT * FROM [dbo].[Transaction]
                WHERE ID = @ID
            ";

            return await _connection.QueryAsync<Transaction>(
                sql,
                new { ID = categoryId },
                _transaction
            );
        }

        public async Task<Transaction?> GetByIdAsync(int id)
        {
            const string sql = @"
                SELECT * FROM [dbo].[Transaction]
                WHERE ID = @ID
            ";

            return await _connection.QueryFirstOrDefaultAsync<Transaction>(
                sql,
                new { ID = id },
                _transaction
            );
        }

        public async Task<IEnumerable<Transaction>> GetByUserIdAsync(Ulid userId)
        {
            const string sql = @"
                SELECT * FROM [dbo].[Transaction]
                WHERE UserId = @UserId
            ";

            return await _connection.QueryAsync<Transaction>(
                sql, 
                new { UserId = userId.ToString() },
                _transaction
            );
        }

        public async Task<Transaction> UpdateAsync(Transaction transaction)
        {
            const string sql = @"
                UPDATE [dbo].[Transaction]
                SET Name = @Name, Description = @Description, Amount = @Amount, CategoryId = @CategoryId
                OUTPUT INSERTED.*
                WHERE ID = @ID
            ";

            return await _connection.QuerySingleAsync<Transaction>(
                sql,
                new { ID = transaction.Id, Name = transaction.Name, Description = transaction.Description, Amount = transaction.Amount, CategoryId = transaction.CategoryId },
                _transaction
            );
        }

        public async Task<IEnumerable<Transaction>> GetByUserIdAndDateRangeAsync(Ulid userId, DateTime startDate, DateTime endDate)
        {
            const string sql = @"
                SELECT * FROM [dbo].[Transaction]
                WHERE UserId = @UserId AND CreatedAt BETWEEN @StartDate AND @EndDate
            ";

            return await _connection.QueryAsync<Transaction>(
                sql,
                new { UserId = userId.ToString(), StartDate = startDate, EndDate = endDate },
                _transaction
            );
        }
    }
}
