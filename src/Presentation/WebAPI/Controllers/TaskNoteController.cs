using Application.DTOs.TaskNote;
using Application.Interfaces.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiVersion(1.0)]
[Authorize]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class TaskNoteController : ControllerBase
{
    private readonly ITaskNoteService _taskNoteService;

    public TaskNoteController(ITaskNoteService taskNoteService)
    {
        _taskNoteService = taskNoteService ?? throw new ArgumentNullException(nameof(taskNoteService));
    }

    [HttpGet("{taskItemId:guid}")]
    public async Task<IActionResult> Get(Guid taskItemId, int pageNumber, int pageQuantity)
    {
        var notes = await _taskNoteService.GetAllAsync(taskItemId, pageNumber, pageQuantity);

        return Ok(notes);
    }
    
    [HttpPost("{taskItemId:guid}")]
    public async Task<IActionResult> Post(Guid taskItemId, CreateTaskNoteDTO dto)
    {
        await _taskNoteService.CreateAsync(taskItemId, dto);

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, UpdateTaskNoteDTO dto)
    {
        await _taskNoteService.UpdateAsync(id, dto);

        return NoContent();
    }

    [HttpPatch("{id:int}/mark-as-done")]
    public async Task<IActionResult> MarkAsDone(int id)
    {
        await _taskNoteService.MarkAsDoneAsync(id);
        
        return NoContent();
    }
    
    [HttpPatch("{id:int}/mark-un-done")]
    public async Task<IActionResult> MarkAsUnDone(int id)
    {
        await _taskNoteService.MarkAsUnDoneAsync(id);
        
        return NoContent();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _taskNoteService.DeleteAsync(id);
        
        return NoContent();
    }
}
