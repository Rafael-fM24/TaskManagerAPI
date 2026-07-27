using Application.DTOs.TaskItem;

namespace Application.Interfaces;

public interface ITaskItemService
{
    IReadOnlyList<TaskItemDTO> GetByUserId(Guid userId);
    Task Create(Guid userId,CreateTaskItemDTO dto);
    void Update(Guid id,UpdateTaskItemDTO dto);
    void Delete(Guid id);
}