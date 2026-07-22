using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class TaskNoteRepository :  ITaskNoteRepository
{
    private readonly AppDbContext _context;

    public TaskNoteRepository(AppDbContext context)
    {
        _context =  context;
    }
    
    public void Add(TaskNote taskNote)
    {
        _context.TaskNotes.Add(taskNote);
        _context.SaveChanges();
    }

    public IReadOnlyList<TaskNote> GetAllNotes(Guid taskItemId)
    {
        return _context.TaskNotes.Where(x => x.TaskItemId == taskItemId).ToList();
    }
}