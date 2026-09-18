using Application.DTOs.TaskNote;

namespace Application.Interfaces.Services;

public interface ITaskNoteService
{
    Task CreateAsync(Guid taskItemId, CreateTaskNoteDTO dto);
    
    Task UpdateAsync(Guid taskItemId, int id, UpdateTaskNoteDTO dto);
    
    Task DeleteAsync(Guid taskItemId,int id);
    
    Task MarkAsDoneAsync(
        Guid taskItemId,
        int noteId);

    Task MarkAsUndoneAsync(
        Guid taskItemId,
        int noteId);
}