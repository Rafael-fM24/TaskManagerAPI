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

    public TaskItemService(ITaskItemRepository taskItemRepository, IMapper mapper, ICurrentUserService currentUserService)
    {
        _taskItemRepository = taskItemRepository;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public IReadOnlyList<TaskItemDTO> GetAllTasks()
    {
        var userId = _currentUserService.UserId;
        
        var taskItems = _taskItemRepository.GetByUserId(userId);

        return _mapper.Map<IReadOnlyList<TaskItemDTO>>(taskItems);
    }

    public async Task Create(CreateTaskItemDTO dto)
    {
        var user = _currentUserService.UserId;

        var taskItem = new TaskItem(
            user,
            dto.Title,
            dto.Description,
            dto.DueDate,
            dto.Priority);
        
        _taskItemRepository.Add(taskItem);
    }

    public void Update(Guid id, UpdateTaskItemDTO dto)
    {
        var task = _taskItemRepository.GetById(id);

        if (task == null)
            throw new Exception("Task not found.");

        task.Update(
            dto.Title,
            dto.Description,
            dto.DueDate,
            dto.Priority);

        _taskItemRepository.Update(task);
    }

    public void Complete(Guid id)
    {
        var task = _taskItemRepository.GetById(id);

        if (task == null)
            throw new DomainException("Tarefa não encontrada.");

        task.Complete();

        _taskItemRepository.Update(task);
    }

    public void Delete(Guid id)
    {
        _taskItemRepository.Remove(id);
    }
}