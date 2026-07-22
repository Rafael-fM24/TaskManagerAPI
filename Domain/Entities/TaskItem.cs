using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("TaskItem")]
public class TaskItem
{
    [Key]
    public Guid Id { get; private set; }

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
    
    public TaskItem(string title, string description)
    {
        Id = Guid.NewGuid();
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