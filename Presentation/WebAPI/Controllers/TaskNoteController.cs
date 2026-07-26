using Application.DTOs.TaskNote;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TaskNoteController : ControllerBase
{
    private readonly ITaskNoteService _taskNoteService;

    public TaskNoteController(ITaskNoteService taskNoteService)
    {
        _taskNoteService = taskNoteService ?? throw new ArgumentNullException(nameof(taskNoteService));
    }

    [HttpGet("{taskItemId:guid}")]
    public IActionResult Get(Guid taskItemId)
    {
        var notes = _taskNoteService.GetAll(taskItemId);

        return Ok(notes);
    }
    
    [HttpPost("{taskItemId:guid}")]
    public IActionResult Post(Guid taskItemId, CreateTaskNoteDTO dto)
    {
        _taskNoteService.Create(taskItemId, dto);

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut("{id:int}")]
    public IActionResult Put(int id, UpdateTaskNoteDTO dto)
    {
        _taskNoteService.Update(id, dto);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        _taskNoteService.Delete(id);
        
        return NoContent();
    }
}
