using Application.DTOs.TaskItem;
using Application.Interfaces.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiVersion(1.0)]
[Authorize]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class TaskItemController : ControllerBase
{
    private readonly ITaskItemService _taskItemService;

    public TaskItemController(ITaskItemService taskItemService)
    {
        _taskItemService = taskItemService ?? throw new ArgumentNullException(nameof(taskItemService));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetMyTasks(int pageNumber, int pageQuantity)
    {
        var tasks = await _taskItemService.GetAllTasksAsync(pageNumber, pageQuantity);
        
        return Ok(tasks);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateTaskItemDTO dto)
    {
        await _taskItemService.CreateAsync(dto);

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, UpdateTaskItemDTO dto)
    {
        await _taskItemService.UpdateAsync(id, dto);

        return NoContent();
    }
    
    [HttpPatch("{id:guid}/complete")]
    public async Task<IActionResult> Complete(Guid id)
    {
        await _taskItemService.CompletedAsync(id);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _taskItemService.DeleteAsync(id);

        return NoContent();
    }
}