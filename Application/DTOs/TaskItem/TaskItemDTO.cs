namespace Application.DTOs.TaskItem;

public class TaskItemDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public bool Completed { get; set; }
    public DateTime Created { get; set; }
    public int Priority { get; set; }
}