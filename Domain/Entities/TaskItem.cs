namespace Domain.Entities;

public class TaskItem
{
    public Guid Id { get; private set; }
    
    public Guid UserId { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;
    
    public bool Completed { get; private set; }
    
    public DateTime Created { get; private set; }
    
    public DateTime? DueDate { get; private set; }
    
    public int Priority { get; private set; }
    
    public ICollection<TaskNote> Notes { get; private set; } = new List<TaskNote>(); 

    private TaskItem()
    {
    }
    
    public TaskItem(Guid userId,string title, string description)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Title = title;
        Description = description;
        Completed = false;
        Created = DateTime.UtcNow;
        Priority = 0;
    }
    
    public void Complete()
    {
        Completed = true;
    }
}