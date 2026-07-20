using Application.DTOs.TaskNote;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskNoteController : ControllerBase
{
    private readonly ITaskRepository _repository;
    private readonly IMapper _mapper;

    public TaskNoteController(ITaskRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public IActionResult Get(Guid taskItemId)
    {
        var taskItem = _repository.GetById(taskItemId);
        
        var taskNoteDTO = _mapper.Map<IReadOnlyList<TaskNoteDTO>>(taskItem.Notes);
        
        return Ok(taskNoteDTO);
    }

    [HttpPost]
    public IActionResult Post(Guid taskItemId, TaskNoteDTO dto)
    {
        var task = _repository.GetById(taskItemId);
        
        if (task == null)
        {
            return NotFound("Task não encontrada.");
        }
        
        var note = new TaskNote(taskItemId, dto.Note);

        _repository.Add(note);

        return Ok();
    }
}
