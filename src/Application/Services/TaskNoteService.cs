using Application.DTOs.TaskNote;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;

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

    public async Task<IReadOnlyList<TaskNoteDTO>> GetAllAsync(Guid taskItemId, int pageNumber, int pageQuantity)
    {
        var notes = await _taskNoteRepository.GetAllNotesAsync(taskItemId, pageNumber, pageQuantity);

        return _mapper.Map<IReadOnlyList<TaskNoteDTO>>(notes);
    }

    public async Task CreateAsync(Guid taskItemId, CreateTaskNoteDTO dto)
    {
        var task = await _taskItemRepository.GetByIdAsync(taskItemId);

        if (task == null)
            throw new NotFoundException("Task not found");

        var note = new TaskNote(taskItemId, dto.Note);

        _taskNoteRepository.Add(note);
        await _taskNoteRepository.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var note = await _taskNoteRepository.GetByIdAsync(id);

        if (note == null)
            throw new NotFoundException("Task not found");
        
        _taskNoteRepository.Remove(note);
        await _taskNoteRepository.SaveAsync();
    }

    public async Task UpdateAsync(int id, UpdateTaskNoteDTO dto)
    {
        var note = await _taskNoteRepository.GetByIdAsync(id);

        if (note == null)
            throw new NotFoundException("Task not found");
        
        note.Update(dto.Note);
        
        await _taskNoteRepository.SaveAsync();
    }
}