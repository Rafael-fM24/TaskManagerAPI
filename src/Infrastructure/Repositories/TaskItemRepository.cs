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

    public async Task<TaskItem?> GetByIdAsync(Guid id, Guid userId)
    {
        return await _context.TaskItems
            .Include(t => t.Notes)
            .FirstOrDefaultAsync(t => 
                t.Id == id &&
                t.UserId == userId);
    }

    public async Task<IReadOnlyList<TaskItem>> GetByUserIdAsync(
        Guid userId, 
        int pageNumber, 
        int pageQuantity)
    {
        return await _context.TaskItems
            .Where(t => t.UserId == userId)
            .OrderBy(t => t.Created)
            .Skip(pageNumber * pageQuantity)
            .Take(pageQuantity)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<TaskNote>> GetNotesAsync(
        Guid taskItemId,
        Guid userId, 
        int pageNumber, 
        int pageQuantity)
    {
        return await _context.TaskNotes
            .Where(n =>
                EF.Property<Guid>(n, "TaskItemId") == taskItemId &&
                _context.TaskItems.Any(t =>
                    t.Id == taskItemId &&
                    t.UserId == userId))
            .OrderBy(n => n.Id)
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