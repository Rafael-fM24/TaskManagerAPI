using Application.DTOs.TaskNote;

namespace Application.Interfaces;

public interface ITaskNoteService
{
    IReadOnlyList<TaskNoteDTO> GetAll(Guid taskItemId);
    void Create(Guid taskItemId, CreateTaskNoteDTO dto);
    void Delete(int id);
    void Update(int id, UpdateTaskNoteDTO dto);
}