using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;
    
    public TaskRepository(AppDbContext context)
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
        return _context.TaskItems.Find(id);
    }
}