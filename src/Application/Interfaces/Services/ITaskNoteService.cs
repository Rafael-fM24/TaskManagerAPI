using Application.DTOs.TaskNote;

namespace Application.Interfaces.Services;

public interface ITaskNoteService
{
    Task<IReadOnlyList<TaskNoteDTO>> GetAllAsync(Guid taskItemId, int pageNumber, int pageQuantity);
    
    Task CreateAsync(Guid taskItemId, CreateTaskNoteDTO dto);
    
    Task DeleteAsync(int id);
    
    Task UpdateAsync(int id, UpdateTaskNoteDTO dto);
}