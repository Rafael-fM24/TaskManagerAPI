using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ITaskNoteRepository
{
    IReadOnlyList<TaskNote> GetAllNotes(Guid taskItemId);
    void Add(TaskNote taskNote);
    void Remove(int id);
    void Update(int id, string note);
    void Save();
}