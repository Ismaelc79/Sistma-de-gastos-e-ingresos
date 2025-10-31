using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction?> GetByIdAsync(int id);
        Task<IEnumerable<Transaction>> GetByUserIdAsync(Ulid userId);
        Task<IEnumerable<Transaction>> GetByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Transaction>> GetByUserIdAndDateRangeAsync(Ulid userId, DateTime startDate, DateTime endDate);
        Task<Transaction> AddAsync(Transaction transaction);
        Task<Transaction> UpdateAsync(Transaction transaction);
        Task<bool> DeleteAsync(int id);
    }
}
