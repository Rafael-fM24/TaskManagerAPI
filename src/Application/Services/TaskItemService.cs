using Application.DTOs.TaskItem;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.Services;

public class TaskItemService : ITaskItemService
{
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    
    private Guid UserId => _currentUserService.UserId;
    
    private async Task<TaskItem> GetTaskItemAsync(Guid id)
    {
        var taskItem = await _taskItemRepository.GetByIdAsync(id, UserId);

        if (taskItem == null)
            throw new NotFoundException("TaskItem not found");

        return taskItem;
    }

    public TaskItemService(ITaskItemRepository taskItemRepository,
        IMapper mapper, 
        ICurrentUserService currentUserService)
    {
        _taskItemRepository = taskItemRepository;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<IReadOnlyList<TaskItemDTO>> GetAllTasksAsync(int pageNumber, int pageQuantity)
    {
         var taskItems = await _taskItemRepository.GetByUserIdAsync(UserId, pageNumber, pageQuantity);

        return _mapper.Map<IReadOnlyList<TaskItemDTO>>(taskItems);
    }

    public async Task CreateAsync(CreateTaskItemDTO dto)
    {
        var taskItem = new TaskItem(
            UserId,
            dto.Title,
            dto.Description,
            dto.DueDate,
            dto.Priority);
        
        _taskItemRepository.Add(taskItem);
        await _taskItemRepository.SaveAsync();
    }

    public async Task UpdateAsync(Guid id, UpdateTaskItemDTO dto)
    {
        var taskItem = await GetTaskItemAsync(id);
        
        taskItem.Update(
            dto.Title,
            dto.Description,
            dto.DueDate,
            dto.Priority);
        
        await _taskItemRepository.SaveAsync();
    }
    
    public async Task InProgressAsync(Guid id)
    {
        var taskItem = await GetTaskItemAsync(id);
        
        taskItem.InProgress();
        
        await _taskItemRepository.SaveAsync();
    }

    public async Task CompletedAsync(Guid id)
    {
        var taskItem = await GetTaskItemAsync(id);
        
        taskItem.Complete();
        
        await _taskItemRepository.SaveAsync();
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var taskItem = await GetTaskItemAsync(id);
        
        _taskItemRepository.Remove(taskItem);
        await _taskItemRepository.SaveAsync();
    }
}