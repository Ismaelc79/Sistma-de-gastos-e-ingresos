using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction?> GetByIdAsync(int id);
        Task<IEnumerable<Transaction>> GetByUserIdAsync(Ulid userId);
        Task<IEnumerable<Transaction>> GetByCategoryIdAsync(int categoryId);
        Task AddAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task DeleteAsync(int id);
    }
}
