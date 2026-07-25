using Application.DTOs.TaskItem;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class TaskItemService : ITaskItemService
{
    private readonly IUserRepository _userRepository;
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly IMapper _mapper;

    public TaskItemService(ITaskItemRepository taskItemRepository, IMapper mapper, IUserRepository userRepository)
    {
        _taskItemRepository = taskItemRepository;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public IReadOnlyList<TaskItemDTO> GetByUserId(Guid userId)
    {
        var taskItems = _taskItemRepository.GetByUserId(userId);

        return _mapper.Map<IReadOnlyList<TaskItemDTO>>(taskItems);
    }

    public async Task Create(CreateTaskItemDTO dto)
    {
        var user = await _userRepository.GetByIdAsync(dto.UserId);

        if (user == null)
            throw new Exception("Usuário não encontrado.");

        var taskItem = _mapper.Map<TaskItem>(dto);

        _taskItemRepository.Add(taskItem);
    }
}