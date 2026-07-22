namespace Application.DTOs.TaskItem;

public class TaskItemDTO
{
    public string Title { get; init; }
    public string Description { get; init; }
    public bool Completed { get; init; }
    public DateTime Created { get; init; }
    public int Priority { get; init; }
}