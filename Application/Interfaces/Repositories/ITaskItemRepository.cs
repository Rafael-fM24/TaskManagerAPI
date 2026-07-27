using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ITaskItemRepository
{
    void Add(TaskItem taskItem);
    void Update(Guid id, string title, string description, DateTime dueDate, int priority);
    void Remove(Guid id);
    TaskItem? GetById(Guid id);
    IReadOnlyList<TaskItem> GetByUserId(Guid userId);
}