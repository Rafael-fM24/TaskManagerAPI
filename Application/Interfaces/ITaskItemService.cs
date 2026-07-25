using Application.DTOs.TaskItem;

namespace Application.Interfaces;

public interface ITaskItemService
{
    IReadOnlyList<TaskItemDTO> GetByUserId(Guid userId);
    Task Create(CreateTaskItemDTO dto);
}