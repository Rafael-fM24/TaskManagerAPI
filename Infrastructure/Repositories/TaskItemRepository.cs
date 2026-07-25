using Domain.Entities;
using Domain.Interfaces;
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
    
    public void Add(TaskItem taskItem)
    {
        _context.TaskItems.Add(taskItem);
        _context.SaveChanges();
    }

    public IReadOnlyList<TaskItem> GetAll()
    {
        return _context.TaskItems.ToList();
    }

    public TaskItem? GetById(Guid id)
    {
        return _context.TaskItems
            .Include(t => t.Notes)
            .FirstOrDefault(t => t.Id == id);
    }

    public IReadOnlyList<TaskItem> GetByUserId(Guid userId)
    {
        return _context.TaskItems
            .Where(t => t.UserId == userId)
            .ToList();
    }
}