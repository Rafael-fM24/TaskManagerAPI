using Application.DTOs.TaskNote;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class TaskNoteService : ITaskNoteService
{
    private readonly ITaskNoteRepository _taskNoteRepository;
    public readonly ITaskItemRepository _taskItemRepository;
    private readonly IMapper _mapper;

    public TaskNoteService(ITaskNoteRepository taskNoteRepository, ITaskItemRepository taskItemRepository, IMapper mapper)
    {
        _taskNoteRepository = taskNoteRepository ??  throw new ArgumentNullException(nameof(taskNoteRepository));
        _taskItemRepository = taskItemRepository ?? throw new ArgumentNullException(nameof(taskItemRepository));
        _mapper = mapper ??  throw new ArgumentNullException(nameof(mapper));
    }

    public IReadOnlyList<TaskNoteDTO> GetAll(Guid taskItemId, int pageNumber, int pageQuantity)
    {
        var notes = _taskNoteRepository.GetAllNotes(taskItemId, pageNumber, pageQuantity);

        return _mapper.Map<IReadOnlyList<TaskNoteDTO>>(notes);
    }

    public void Create(Guid taskItemId, CreateTaskNoteDTO dto)
    {
        var task = _taskItemRepository.GetById(taskItemId);

        if (task == null)
            throw new Exception("Task not found");

        var note = new TaskNote(taskItemId, dto.Note);

        _taskNoteRepository.Add(note);
        _taskNoteRepository.Save();
    }

    public void Delete(int id)
    {
        _taskNoteRepository.Remove(id);
        _taskNoteRepository.Save();
    }

    public void Update(int id, UpdateTaskNoteDTO dto)
    {
        _taskNoteRepository.Update(id, dto.Note);
        _taskNoteRepository.Save();
    }
}