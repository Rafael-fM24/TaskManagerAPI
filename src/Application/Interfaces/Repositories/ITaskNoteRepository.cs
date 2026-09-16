using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ITaskNoteRepository
{
    Task<TaskNote?> GetByIdAsync(int id, Guid userId);
    
    Task<IReadOnlyList<TaskNote>> GetAllNotesAsync(Guid taskItemId,int pageNumber, int pageQuantity);
    
    Task<bool> AllNotesDoneAsync(Guid taskItemId, int currentNoteId);
    
    void Add(TaskNote taskNote);
    
    void Remove(TaskNote taskNote);
    
    Task SaveAsync();
}