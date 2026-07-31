using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ITaskItemRepository
{
    void Add(TaskItem taskItem);
    void Update(TaskItem taskItem);
    void Remove(Guid id);
    TaskItem? GetById(Guid id);
    IReadOnlyList<TaskItem> GetByUserId(Guid userId);
}