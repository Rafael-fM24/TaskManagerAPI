using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TaskNoteRepository :  ITaskNoteRepository
{
    private readonly AppDbContext _context;

    public TaskNoteRepository(AppDbContext context)
    {
        _context =  context;
    }

    public async Task<TaskNote?> GetByIdAsync(int id, Guid userId)
    {
        return await _context.TaskNotes
            .FirstOrDefaultAsync(n =>
                n.Id == id &&
                _context.TaskItems.Any(t =>
                    t.Id == n.TaskItemId &&
                    t.UserId == userId));
    }

    public async Task<IReadOnlyList<TaskNote>> GetAllNotesAsync(Guid taskItemId, int pageNumber, int pageQuantity)
    {
        return await _context.TaskNotes
            .Where(x => x.TaskItemId == taskItemId)
            .OrderBy(x => x.Id)
            .Skip(pageNumber * pageQuantity)
            .Take(pageQuantity)
            .ToListAsync();
    }
    
    public async Task<bool> AllNotesDoneAsync(Guid taskItemId, int currentNoteId)
    {
        return !await _context.TaskNotes
            .AnyAsync(n =>
                n.TaskItemId == taskItemId &&
                n.Id != currentNoteId &&
                !n.Done);
    }

    public void Add(TaskNote taskNote)
    { 
        _context.TaskNotes.Add(taskNote);
    }

    public void Remove(TaskNote taskNote)
    {
        _context.TaskNotes.Remove(taskNote);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}