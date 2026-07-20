using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
        return _context.TaskItems
            .Include(t => t.Notes)
            .FirstOrDefault(t => t.Id == id);
    }

    public IReadOnlyList<TaskNote> GetAllNotes(Guid taskItemId)
    {
        return _context.TaskNotes.Where(x => x.TaskItemId == taskItemId).ToList();
    }

    public void Add(TaskNote taskNote)
    {
        _context.TaskNotes.Add(taskNote);
        _context.SaveChanges();
    }
}