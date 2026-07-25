using Domain.Entities;

namespace Domain.Interfaces;

public interface ITaskItemRepository
{
    //TaskItem
    void Add(TaskItem taskItem);
    IReadOnlyList<TaskItem> GetAll();
    TaskItem? GetById(Guid id);
    IReadOnlyList<TaskItem> GetByUserId(Guid userId);
}