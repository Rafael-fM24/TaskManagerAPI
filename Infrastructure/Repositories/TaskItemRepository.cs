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
    
    public void Add(TaskItem taskItem)
    {
        _context.TaskItems.Add(taskItem);
        _context.SaveChanges();
    }

    public void Update(Guid id, string title, string description, DateTime dueDate, int priority)
    {
        var taskItem = _context.TaskItems.Find(id);
        
        if (taskItem == null)
            throw new Exception("TaskItem não encontrada.");

        taskItem.Update(title, description, dueDate, priority);

        _context.SaveChanges();
    }

    public void Remove(Guid id)
    {
        var taskItem = _context.TaskItems.Find(id);
        
        if (taskItem == null)
            throw new Exception("TaskItem não existe.");
        
        _context.TaskItems.Remove(taskItem);
        _context.SaveChanges();
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