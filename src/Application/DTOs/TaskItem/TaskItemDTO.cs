using Domain.Enums;

namespace Application.DTOs.TaskItem;

public class TaskItemDTO
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public bool Completed { get; init; }
    public DateTime Created { get; init; }
    public DateTime DueDate { get; init; }
    public PriorityLevel Priority { get; init; }
}