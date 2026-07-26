using Application.DTOs.TaskItem;
using Application.DTOs.TaskNote;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

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

    public IReadOnlyList<TaskNoteDTO> GetAll(Guid taskItemId)
    {
        var notes = _taskNoteRepository.GetAllNotes(taskItemId);

        return _mapper.Map<IReadOnlyList<TaskNoteDTO>>(notes);
    }

    public void Create(Guid taskItemId, CreateTaskNoteDTO dto)
    {
        var task = _taskItemRepository.GetById(taskItemId);

        if (task == null)
            throw new Exception("Task não encontrada.");

        var note = new TaskNote(taskItemId, dto.Note);

        _taskNoteRepository.Add(note);
    }

    public bool Delete(int id)
    {
        return _taskNoteRepository.RemoveById(id);
    }
}