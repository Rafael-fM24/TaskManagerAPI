using System.Security.Claims;
using Application.DTOs.TaskItem;
using Application.Interfaces;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TaskItemController : ControllerBase
{
    private readonly ITaskItemService _taskItemService;

    public TaskItemController(ITaskItemService taskItemService)
    {
        _taskItemService = taskItemService ?? throw new ArgumentNullException(nameof(taskItemService));
    }
    
    [HttpGet]
    public IActionResult GetMyTasks()
    {
        var tasks = _taskItemService.GetAllTasks();

        return Ok(tasks);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateTaskItemDTO dto)
    {
        await _taskItemService.Create(dto);

        return Ok();
    }

    [HttpPut("{id:guid}")]
    public IActionResult Put(Guid id, UpdateTaskItemDTO dto)
    {
        _taskItemService.Update(id, dto);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        _taskItemService.Delete(id);

        return NoContent();
    }
}