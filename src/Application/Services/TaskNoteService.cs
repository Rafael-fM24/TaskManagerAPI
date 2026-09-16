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
    
    private Guid UserId => _currentUserService.UserId;

    private async Task<TaskNote> GetTaskNoteAsync(int id)
    {
        var taskNote = await _taskNoteRepository.GetByIdAsync(id, UserId);

        if (taskNote is null)
            throw new NotFoundException("TaskNote not found.");

        return taskNote;
    }
    
    private async Task<TaskItem> GetTaskItemAsync(Guid id)
    {
        var taskItem = await _taskItemRepository.GetByIdAsync(id, UserId);

        if (taskItem == null)
            throw new NotFoundException("TaskItem not found");

        return taskItem;
    }

    public TaskNoteService(ITaskNoteRepository taskNoteRepository, ITaskItemRepository taskItemRepository, ICurrentUserService currentUserService,IMapper mapper)
    {
        _taskNoteRepository = taskNoteRepository ?? throw new ArgumentNullException(nameof(taskNoteRepository));
        _taskItemRepository = taskItemRepository ?? throw new ArgumentNullException(nameof(taskItemRepository));
        _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IReadOnlyList<TaskNoteDTO>> GetAllAsync(Guid taskItemId, int pageNumber, int pageQuantity)
    {
        await GetTaskItemAsync(taskItemId);

        var notes = await _taskNoteRepository.GetAllNotesAsync(
            taskItemId,
            pageNumber,
            pageQuantity);

        return _mapper.Map<IReadOnlyList<TaskNoteDTO>>(notes);
    }

    public async Task CreateAsync(Guid taskItemId, CreateTaskNoteDTO dto)
    {
        await GetTaskItemAsync(taskItemId);

        var note = new TaskNote(taskItemId, dto.Note);

        _taskNoteRepository.Add(note);
        await _taskNoteRepository.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var note = await GetTaskNoteAsync(id);
        
        _taskNoteRepository.Remove(note);
        await _taskNoteRepository.SaveAsync();
    }

    public async Task UpdateAsync(int id, UpdateTaskNoteDTO dto)
    {
        var note = await GetTaskNoteAsync(id);
        
        note.Update(dto.Note);
        
        await _taskNoteRepository.SaveAsync();
    }

    public async Task MarkAsDoneAsync(int id)
    {
        var taskNote = await GetTaskNoteAsync(id);

        taskNote.MarkAsDone();

        var taskItem = await GetTaskItemAsync(taskNote.TaskItemId);

        var allNotesDone =
            await _taskNoteRepository.AllNotesDoneAsync(taskItem.Id, id);

        if (allNotesDone)
        {
            taskItem.Complete();
        }
        else
        {
            taskItem.InProgress();
        }

        await _taskNoteRepository.SaveAsync();
    }

    public async Task MarkAsUnDoneAsync(int id)
    {
        var taskNote = await _taskNoteRepository.GetByIdAsync(id, UserId);

        if (taskNote is null)
            throw new NotFoundException("TaskNote not found.");

        taskNote.MarkAsUnDone();
        
        var taskItem = await GetTaskItemAsync(taskNote.TaskItemId);
        
       taskItem.InProgress();

        await _taskNoteRepository.SaveAsync();
    }
}