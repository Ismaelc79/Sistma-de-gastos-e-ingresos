using Application.DTOs.Transactions;

namespace Application.Interfaces;

public interface ITransactionService
{
    Task<TransactionDto> CreateTransactionAsync(CreateTransactionRequest request);
    Task<TransactionDto?> GetTransactionByIdAsync(int id);
    Task<IEnumerable<TransactionDto>> GetTransactionsByUserIdAsync(string userId);
    Task<IEnumerable<TransactionDto>> GetTransactionsByUserIdAndDateRangeAsync(string userId, DateTime startDate, DateTime endDate);
    Task<IEnumerable<TransactionDto>> GetTransactionsByCategoryIdAsync(int categoryId);
    Task<TransactionDto> UpdateTransactionAsync(int id, CreateTransactionRequest request);
    Task<bool> DeleteTransactionAsync(int id);
}
