using Application.DTOs.TaskNote;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TaskNoteController : ControllerBase
{
    private readonly ITaskNoteRepository _taskNoteRepository;
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly IMapper _mapper;

    public TaskNoteController(ITaskNoteRepository taskNoteRepository, ITaskItemRepository taskItemRepository, IMapper mapper)
    {
        _taskNoteRepository = taskNoteRepository ?? throw new ArgumentNullException(nameof(taskNoteRepository));
        _taskItemRepository = taskItemRepository ?? throw new ArgumentNullException(nameof(taskItemRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet("{taskItemId:guid}")]
    public IActionResult Get(Guid taskItemId)
    {
        var notes = _taskNoteRepository.GetAllNotes(taskItemId);
        
        var taskNoteDTO = _mapper.Map<IReadOnlyList<TaskNoteDTO>>(notes);
        
        return Ok(taskNoteDTO);
    }
    
    [HttpPost("{taskItemId:guid}")]
    public IActionResult Post(Guid taskItemId, TaskNoteDTO dto)
    {
        var task = _taskItemRepository.GetById(taskItemId);
        
        if (task == null)
        {
            return NotFound("Task não encontrada.");
        }
        
        var note = new TaskNote(taskItemId, dto.Note);

        _taskNoteRepository.Add(note);

        return Ok();
    }
}
