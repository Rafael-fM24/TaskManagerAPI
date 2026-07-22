using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.TaskNote;

public class TaskNoteDTO
{
    [Required]
    [MaxLength(1000)]
    public string Note { get; set; } = string.Empty;
}