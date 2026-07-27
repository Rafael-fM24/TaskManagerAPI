using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.TaskItem;

public class UpdateTaskItemDTO
{
    [Required]
    [MaxLength(100)]
    public string Title { get; init; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Description { get; init; } = string.Empty;
    
    public DateTime DueDate { get; init; }
    
    public int Priority { get; init; }
}