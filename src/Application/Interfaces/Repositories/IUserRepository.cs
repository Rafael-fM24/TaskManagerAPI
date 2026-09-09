using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);

    Task<User?> GetByIdAsync(Guid id);
    
    void Add(User user);
    
    void RemoveUser(User user);

    Task SaveAsync();
}