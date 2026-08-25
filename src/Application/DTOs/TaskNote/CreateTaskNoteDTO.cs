using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.TaskNote;

public class CreateTaskNoteDTO
{
    [Required]
    [MaxLength(1000)]
    public string Note { get; init; } = string.Empty;
}