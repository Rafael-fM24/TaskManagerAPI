using Application.DTOs.TaskNote;

namespace Application.Interfaces.Services;

public interface ITaskNoteService
{
    IReadOnlyList<TaskNoteDTO> GetAll(Guid taskItemId, int pageNumber, int pageQuantity);
    void Create(Guid taskItemId, CreateTaskNoteDTO dto);
    void Delete(int id);
    void Update(int id, UpdateTaskNoteDTO dto);
}