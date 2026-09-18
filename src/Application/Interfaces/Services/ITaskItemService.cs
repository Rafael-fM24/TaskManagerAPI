using Application.DTOs.TaskItem;
using Application.DTOs.TaskNote;

namespace Application.Interfaces.Services;

public interface ITaskItemService
{
    Task <IReadOnlyList<TaskItemDTO>> GetAllTasksAsync(int pageNumber, int pageQuantity);
    
    Task<IReadOnlyList<TaskNoteDTO>> GetNotesAsync(
        Guid taskItemId,
        int pageNumber,
        int pageQuantity);
    
    Task CreateAsync(CreateTaskItemDTO dto);
    
    Task UpdateAsync(Guid id,UpdateTaskItemDTO dto);
    
    Task InProgressAsync(Guid id);
    
    Task CompletedAsync(Guid id);
    
    Task DeleteAsync(Guid id);
}