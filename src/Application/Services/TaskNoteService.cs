using Application.DTOs.TaskNote;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Exceptions;

namespace Application.Services;

public class TaskNoteService : ITaskNoteService
{
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    private Guid UserId => _currentUserService.UserId;

    public TaskNoteService(
        ITaskItemRepository taskItemRepository,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _taskItemRepository = taskItemRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task CreateAsync(Guid taskItemId, CreateTaskNoteDTO dto)
    {
        var task = await _taskItemRepository.GetByIdAsync(taskItemId, UserId);

        if (task == null)
            throw new NotFoundException("Task not found.");

        task.AddNote(dto.Note);

        await _taskItemRepository.SaveAsync();
    }

    public async Task UpdateAsync(
        Guid taskItemId,
        int noteId,
        UpdateTaskNoteDTO dto)
    {
        var task = await _taskItemRepository.GetByIdAsync(taskItemId, UserId);

        if (task == null)
            throw new NotFoundException("Task not found.");

        task.UpdateNote(noteId, dto.Note);

        await _taskItemRepository.SaveAsync();
    }
    
    public async Task DeleteAsync(
        Guid taskItemId,
        int noteId)
    {
        var task = await _taskItemRepository.GetByIdAsync(taskItemId, UserId);

        if (task == null)
            throw new NotFoundException("Task not found.");

        task.RemoveNote(noteId);

        await _taskItemRepository.SaveAsync();
    }

    public async Task MarkAsDoneAsync(Guid taskItemId, int noteId)
    {
        var task = await _taskItemRepository
            .GetByIdAsync(taskItemId, UserId);

        if (task == null)
            throw new NotFoundException("Task not found.");

        task.MarkNoteAsDone(noteId);
        
        if(task.AllNotesDone(noteId))
            task.Complete();
        else
            task.InProgress();

        await _taskItemRepository.SaveAsync();
    }

    public async Task MarkAsUndoneAsync(Guid taskItemId, int noteId)
    {
        var task = await _taskItemRepository
            .GetByIdAsync(taskItemId, UserId);

        if (task == null)
            throw new NotFoundException("Task not found.");

        task.MarkNoteAsUndone(noteId);
        
        task.InProgress();

        await _taskItemRepository.SaveAsync();
    }
}