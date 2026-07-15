using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskItemController : ControllerBase
{
    private readonly ITaskRepository _repository;

    public TaskItemController(ITaskRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    [HttpGet]
    public IActionResult Get()
    {
        var taskItems = _repository.GetAll();
        return Ok(taskItems);
    }

    [HttpPost]
    public IActionResult Post(TaskItem taskItem)
    {
        _repository.Add(taskItem);
        return Ok();
    }
}