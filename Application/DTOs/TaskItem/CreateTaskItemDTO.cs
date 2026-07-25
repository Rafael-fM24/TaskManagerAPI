using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.TaskItem;

public class CreateTaskItemDTO
{
    [Required]
    public string Title { get; init; } = string.Empty;

    [Required] 
    public string Description { get; init; } = string.Empty;
    
    public DateTime DueDate { get; init; }
    
    public int Priority { get; init; }
}