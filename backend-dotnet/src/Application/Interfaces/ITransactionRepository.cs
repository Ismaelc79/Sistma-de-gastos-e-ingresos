using Domain.Entities;

namespace Application.Interfaces;

public interface ITransactionRepository : IRepository<Transaction>
{
    Task<IEnumerable<Transaction>> GetByUserIdAsync(string userId);
    Task<IEnumerable<Transaction>> GetByCategoryIdAsync(int categoryId);
    Task<IEnumerable<Transaction>> GetByUserIdAndDateRangeAsync(string userId, DateTime startDate, DateTime endDate);
    Task<IEnumerable<Transaction>> GetByUserIdAndCategoryAsync(string userId, int categoryId);
}
