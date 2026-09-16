using Application.DTOs.TaskItem;

namespace Application.Interfaces.Services;

public interface ITaskItemService
{
    Task <IReadOnlyList<TaskItemDTO>> GetAllTasksAsync(int pageNumber, int pageQuantity);
    
    Task CreateAsync(CreateTaskItemDTO dto);
    
    Task UpdateAsync(Guid id,UpdateTaskItemDTO dto);
    
    Task InProgressAsync(Guid id);
    
    Task CompletedAsync(Guid id);
    
    Task DeleteAsync(Guid id);
}