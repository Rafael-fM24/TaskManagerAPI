using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ITaskNoteRepository
{
    IReadOnlyList<TaskNote> GetAllNotes(Guid taskItemId, int pageNumber, int pageQuantity);
    void Add(TaskNote taskNote);
    void Remove(int id);
    void Update(int id, string note);
    void Save();
}