using Domain.Entities;

namespace Domain.Interfaces;

public interface ITaskNoteRepository
{
    void Add(TaskNote taskNote);
    IReadOnlyList<TaskNote> GetAllNotes(Guid taskItemId);
    void RemoveById(int id);
    void Update(int id, string note);
}