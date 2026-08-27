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
    public IActionResult GetMyTasks(int pageNumber, int pageQuantity)
    {
        var tasks = _taskItemService.GetAllTasks(pageNumber, pageQuantity);
        
        return Ok(tasks);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateTaskItemDTO dto)
    {
        await _taskItemService.Create(dto);

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut("{id:guid}")]
    public IActionResult Put(Guid id, UpdateTaskItemDTO dto)
    {
        _taskItemService.Update(id, dto);

        return NoContent();
    }
    
    [HttpPatch("{id:guid}/complete")]
    public IActionResult Complete(Guid id)
    {
        _taskItemService.Complete(id);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        _taskItemService.Delete(id);

        return NoContent();
    }
}