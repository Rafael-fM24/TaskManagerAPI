using Application.DTOs.TaskItem;
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
public class TaskItemController : ControllerBase
{
    private readonly ITaskItemService _taskItemService;
    private readonly ITaskNoteService _taskNoteService;

    public TaskItemController(ITaskItemService taskItemService, ITaskNoteService taskNoteService)
    {
        _taskItemService = taskItemService ?? throw new ArgumentNullException(nameof(taskItemService));
        _taskNoteService = taskNoteService ?? throw new ArgumentNullException(nameof(taskNoteService));
    }
    
    
    // task
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
    
    [HttpPatch("{id:guid}/in-progress")]
    public async Task<IActionResult> InProgress(Guid id)
    {
        await _taskItemService.InProgressAsync(id);

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

    
    // note
    [HttpGet("{taskItemId:guid}/Notes")]
    public async Task<IActionResult> GetNotes(Guid taskItemId, int pageNumber, int pageQuantity)
    {
        var notes = await _taskItemService.GetNotesAsync(
            taskItemId,
            pageNumber,
            pageQuantity);

        return Ok(notes);
    }

    [HttpPost("{taskItemId:guid}/Notes")]
    public async Task<IActionResult> CreateNote(Guid taskItemId, CreateTaskNoteDTO dto)
    {
        await _taskNoteService.CreateAsync(taskItemId, dto);
        
        return StatusCode(StatusCodes.Status201Created);
    }
    
    [HttpPut("{taskItemId:guid}/notes/{id:int}")]
     public async Task<IActionResult> UpdateNote(Guid taskItemId, int id, UpdateTaskNoteDTO dto)
     {
         await _taskNoteService.UpdateAsync(taskItemId, id, dto);

         return NoContent();
     }
     
    [HttpPatch("{taskItemId:guid}/notes/{id:int}/mark-as-done")]
     public async Task<IActionResult> MarkAsDone(Guid taskItemId, int id)
     {
         await _taskNoteService.MarkAsDoneAsync(taskItemId, id);
         
         return NoContent();
     }
     
     [HttpPatch("{taskItemId:guid}/notes/{id:int}/mark-un-done")]
     public async Task<IActionResult> MarkAsUnDone(Guid taskItemId, int id)
     {
         await _taskNoteService.MarkAsUndoneAsync(taskItemId, id);
         
         return NoContent();
     }
     
     [HttpDelete("{taskItemId:guid}/notes/{id:int}")]
     public async Task<IActionResult> DeleteNote(Guid taskItemId, int id)
     {
         await _taskNoteService.DeleteAsync(taskItemId, id);
         
         return NoContent();
     }
}