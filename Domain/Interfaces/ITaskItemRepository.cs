using Domain.Entities;

namespace Domain.Interfaces;

public interface ITaskItemRepository
{
    void Add(TaskItem taskItem);
    void Update(Guid id, string title, string description, DateTime dueDate, int priority);
    void Remove(Guid id);
    IReadOnlyList<TaskItem> GetAll();
    TaskItem? GetById(Guid id);
    IReadOnlyList<TaskItem> GetByUserId(Guid userId);
}