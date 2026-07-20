using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("TaskItem")]
public class TaskItem
{
    [Key]
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool Completed { get; set; }
    public DateTime Created { get; set; }
    public DateTime DueDate { get; set; }
    public int Priority { get; set; }
    public ICollection<TaskNote> Notes { get; set; }

    public TaskItem(string title, string description)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        Completed = false;
        Created = DateTime.UtcNow;
        Priority = 0;
    }
}