using Domain.Entities;

namespace Domain.Interfaces;

public interface ITaskRepository
{
    void Add(TaskItem taskItem);
    
    List<TaskItem> GetAll();
    public TaskItem? GetById(Guid id);
}