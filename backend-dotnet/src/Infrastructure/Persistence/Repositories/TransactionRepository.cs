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

        public async Task AddAsync(Transaction transaction)
        {
            const string sql = @"
                INSERT INTO dbo.Transaction (CategoryId, UserId, Name, Description) 
                VALUES (@CategoryId, @UserId, @Name, @Description)
            ";

            await _connection.ExecuteAsync(
                sql,
                new { CategoryId = transaction.CategoryId, UserId = transaction.UserId, Name = transaction.Name, Descripcion = transaction.Description },
                _transaction
            );
        }

        public async Task DeleteAsync(int id)
        {
            const string sql = @"
                DELETE FROM dbo.Transaction 
                WHERE ID = @ID
            ";

            await _connection.ExecuteAsync(
                sql,
                new { ID = id },
                _transaction
            );
        }

        public async Task<IEnumerable<Transaction>> GetByCategoryIdAsync(int categoryId)
        {
            const string sql = @"
                SELECT * FROM dbo.Transaction
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
                SELECT * FROM dbo.Transaction
                WHERE ID = @ID
            ";

            return await _connection.QueryFirstOrDefaultAsync<Transaction>(
                sql,
                new { ID = id },
                _transaction
            );
        }

        public async Task<IEnumerable<Transaction>> GetByUserIdAsync(string userId)
        {
            const string sql = @"
                SELECT * FROM dbo.Transaction
                WHERE UserId = @UserId
            ";

            return await _connection.QueryAsync<Transaction>(
                sql, 
                new { UserId = userId },
                _transaction
            );
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            const string sql = @"
                UPDATE dbo.Transaction
                SET Name = @Name, Description = @Description, Amount = @Amount
                WHERE ID = @ID
            ";

            await _connection.ExecuteAsync(
                sql,
                new { ID = transaction.Id, Name = transaction.Name, Description = transaction.Description, Amount = transaction.Amount },
                _transaction
            );
        }
    }
}
