using Domain.Entities;

namespace Domain.Interfaces;

public interface ITaskItemRepository
{
    //TaskItem
    void Add(TaskItem taskItem);
    IReadOnlyList<TaskItem> GetAll();
    public TaskItem? GetById(Guid id);
}