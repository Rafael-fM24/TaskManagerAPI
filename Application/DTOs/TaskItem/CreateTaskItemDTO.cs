using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.TaskItem;

public class CreateTaskItemDTO
{
    public Guid Id { get; init; }
    
    [Required]
    public string Title { get; init; } = string.Empty;

    [Required] 
    public string Description { get; init; } = string.Empty;
}