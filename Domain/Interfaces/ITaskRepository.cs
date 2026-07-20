using Domain.Entities;

namespace Domain.Interfaces;

public interface ITaskRepository
{
    //TaskItem
    void Add(TaskItem taskItem);
    IReadOnlyList<TaskItem> GetAll();
    public TaskItem? GetById(Guid id);
    
    //TaskNote
    IReadOnlyList<TaskNote> GetAllNotes(Guid taskItemId);
    void Add(TaskNote taskNote);
}