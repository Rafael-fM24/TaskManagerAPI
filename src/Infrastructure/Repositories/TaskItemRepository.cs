using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TaskItemRepository : ITaskItemRepository
{
    private readonly AppDbContext _context;
    
    public TaskItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id)
    {
        return await _context.TaskItems
            .Include(t => t.Notes)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IReadOnlyList<TaskItem>> GetByUserIdAsync(Guid userId, int pageNumber, int pageQuantity)
    {
        return await _context.TaskItems
            .Where(t => t.UserId == userId)
            .OrderBy(t => t.Created)
            .Skip(pageNumber * pageQuantity)
            .Take(pageQuantity)
            .ToListAsync();
    }

    public void Add(TaskItem taskItem)
    {
        _context.TaskItems.Add(taskItem);
    }
    
    
    public void Remove(TaskItem taskItem)
    {
        _context.TaskItems.Remove(taskItem);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}