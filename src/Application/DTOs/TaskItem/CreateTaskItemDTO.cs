using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.TaskItem;

public class CreateTaskItemDTO
{
    [Required]
    [MaxLength(100)]
    public string Title { get; init; } = string.Empty;

    [Required] 
    [MaxLength(100)]
    public string Description { get; init; } = string.Empty;
    
    public DateTime DueDate { get; init; }
    
    public PriorityLevel Priority { get; init; }
}