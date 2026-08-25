using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task UpdateAsync(Guid id, string name, string email)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            throw new Exception("User not found");

        user.Update(name, email);
    }

    public async Task DeleteUserIdAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        
        if (user is not null)
            _context.Users.Remove(user);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}