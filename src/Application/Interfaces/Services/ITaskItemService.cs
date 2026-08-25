using Application.DTOs.TaskItem;

namespace Application.Interfaces.Services;

public interface ITaskItemService
{
    IReadOnlyList<TaskItemDTO> GetAllTasks();
    Task Create(CreateTaskItemDTO dto);
    void Update(Guid id,UpdateTaskItemDTO dto);
    void Complete(Guid id);
    void Delete(Guid id);
}