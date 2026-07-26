using System.Security.Claims;
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
    
    [HttpGet]
    public IActionResult Get()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(userId, out var id))
            return Unauthorized();

        var tasks = _taskItemService.GetByUserId(id);

        return Ok(tasks);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateTaskItemDTO dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();
        
        await _taskItemService.Create(userId, dto);

        return Ok();
    }
}