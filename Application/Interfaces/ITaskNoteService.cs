using Application.DTOs.TaskNote;

namespace Application.Interfaces;

public interface ITaskNoteService
{
    IReadOnlyList<TaskNoteDTO> GetAll(Guid taskItemId);
    void Create(Guid taskItemId, CreateTaskNoteDTO dto);
}