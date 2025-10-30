namespace Application.Interfaces;

public interface IRepository<T, TId> where T : class
{
    Task<T?> GetByIdAsync(TId id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(TId id);
    Task<bool> ExistsAsync(TId id);
}

// Interfaz genérica con ID int por defecto
public interface IRepository<T> : IRepository<T, int> where T : class
{
}
