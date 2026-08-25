using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class TaskNoteRepository :  ITaskNoteRepository
{
    private readonly AppDbContext _context;

    public TaskNoteRepository(AppDbContext context)
    {
        _context =  context;
    }

    public IReadOnlyList<TaskNote> GetAllNotes(Guid taskItemId)
    {
        return _context.TaskNotes.Where(x => x.TaskItemId == taskItemId).ToList();
    }

    public void Add(TaskNote taskNote)
    {
        _context.TaskNotes.Add(taskNote);
    }

    public void Remove(int id)
    {
        var taskNote = _context.TaskNotes.Find(id);

        if (taskNote == null)
            throw new Exception("TaskNote not found");
        
        _context.TaskNotes.Remove(taskNote);
    }

    public void Update(int id, string note)
    {
        var taskNote =  _context.TaskNotes.Find(id);
        
        if (taskNote == null)
            throw new Exception("TaskNote not found");
        
        taskNote.Update(note);
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}