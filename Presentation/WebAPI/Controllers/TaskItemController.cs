using Application.DTOs.TaskItem;
using Application.Interfaces;
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
    
    [HttpGet("{userId:guid}")]
    public IActionResult Get(Guid userId)
    {
        var tasks = _taskItemService.GetByUserId(userId);

        return Ok(tasks);
    }

    [HttpPost("{userId:guid}")]
    public async Task<IActionResult> Post(CreateTaskItemDTO dto)
    {
        await _taskItemService.Create(dto);

        return Ok();
    }
}