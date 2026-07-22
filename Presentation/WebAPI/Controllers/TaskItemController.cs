using Application.DTOs.TaskItem;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TaskItemController : ControllerBase
{
    private readonly ITaskItemRepository _itemRepository;
    private readonly IMapper _mapper;

    public TaskItemController(ITaskItemRepository itemRepository, IMapper mapper)
    {
        _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
        _mapper = mapper ??  throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public IActionResult Get()
    {
        var taskItems = _itemRepository.GetAll();
        
        var taskItemDTO =  _mapper.Map<IEnumerable<TaskItemDTO>>(taskItems);
            
        return Ok(taskItemDTO);
    }

    [HttpPost]
    public IActionResult Post(CreateTaskItemDTO dto)
    {
        var taskItem = _mapper.Map<TaskItem>(dto);
        
        _itemRepository.Add(taskItem);
        return Ok();
    }
}