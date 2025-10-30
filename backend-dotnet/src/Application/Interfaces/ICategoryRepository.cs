using Domain.Entities;

namespace Application.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<IEnumerable<Category>> GetByUserIdAsync(string userId);
    Task<IEnumerable<Category>> GetDefaultCategoriesAsync();
}
