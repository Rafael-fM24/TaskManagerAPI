using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.TaskItem;

public class CreateTaskItemDTO
{
    public Guid Id { get; set; }
    
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required] 
    public string Description { get; set; } = string.Empty;
}