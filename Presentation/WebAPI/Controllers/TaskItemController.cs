using Application.DTOs.TaskItem;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskItemController : ControllerBase
{
    private readonly ITaskRepository _repository;
    private readonly IMapper _mapper;

    public TaskItemController(ITaskRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ??  throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public IActionResult Get()
    {
        var taskItems = _repository.GetAll();
        
        var taskItemDTO =  _mapper.Map<IEnumerable<TaskItemDTO>>(taskItems);
            
        return Ok(taskItemDTO);
    }

    [HttpPost]
    public IActionResult Post(CreateTaskItemDTO dto)
    {
        var taskItem = _mapper.Map<TaskItem>(dto);
        
        _repository.Add(taskItem);
        return Ok();
    }
}