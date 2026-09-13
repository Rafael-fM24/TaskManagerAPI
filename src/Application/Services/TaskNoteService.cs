using Application.DTOs.TaskNote;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.Services;

public class TaskNoteService : ITaskNoteService
{
    private readonly ITaskNoteRepository _taskNoteRepository;
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public TaskNoteService(ITaskNoteRepository taskNoteRepository, ITaskItemRepository taskItemRepository, ICurrentUserService currentUserService,IMapper mapper)
    {
        _taskNoteRepository = taskNoteRepository ?? throw new ArgumentNullException(nameof(taskNoteRepository));
        _taskItemRepository = taskItemRepository ?? throw new ArgumentNullException(nameof(taskItemRepository));
        _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IReadOnlyList<TaskNoteDTO>> GetAllAsync(Guid taskItemId, int pageNumber, int pageQuantity)
    {
        var userId = _currentUserService.UserId;

        var taskItem = await _taskItemRepository.GetByIdAsync(taskItemId, userId);

        if (taskItem == null)
            throw new NotFoundException("TaskItem not found");

        var notes = await _taskNoteRepository.GetAllNotesAsync(
            taskItemId,
            pageNumber,
            pageQuantity);

        return _mapper.Map<IReadOnlyList<TaskNoteDTO>>(notes);
    }

    public async Task CreateAsync(Guid taskItemId, CreateTaskNoteDTO dto)
    {
        var userId = _currentUserService.UserId;
        
        var taskItem = await _taskItemRepository.GetByIdAsync(taskItemId, userId);

        if (taskItem == null)
            throw new NotFoundException("TaskNote not found");

        var note = new TaskNote(taskItemId, dto.Note);

        _taskNoteRepository.Add(note);
        await _taskNoteRepository.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var userId = _currentUserService.UserId;
        
        var note = await _taskNoteRepository.GetByIdAsync(id, userId);

        if (note == null)
            throw new NotFoundException("TaskNote not found");
        
        _taskNoteRepository.Remove(note);
        await _taskNoteRepository.SaveAsync();
    }

    public async Task UpdateAsync(int id, UpdateTaskNoteDTO dto)
    {
        var userId = _currentUserService.UserId;
        
        var note = await _taskNoteRepository.GetByIdAsync(id, userId);

        if (note == null)
            throw new NotFoundException("TaskNote not found");
        
        note.Update(dto.Note);
        
        await _taskNoteRepository.SaveAsync();
    }
}