using Domain.Entities;

namespace Application.Interfaces;

public interface IUserRepository : IRepository<User, string>
{
    Task<User?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
}
