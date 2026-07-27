using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ITaskNoteRepository
{
    void Add(TaskNote taskNote);
    void Remove(int id);
    void Update(int id, string note);
    IReadOnlyList<TaskNote> GetAllNotes(Guid taskItemId);
}