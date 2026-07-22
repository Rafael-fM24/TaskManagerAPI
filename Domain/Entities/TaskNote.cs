using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("TaskNote")]
public class TaskNote 
{
    [Key]
    public int Id { get; private set; }
    
    public Guid TaskItemId { get; private set; }
    
    public string Note { get; private set; }
    
    [ForeignKey(nameof(TaskItemId))]
    public virtual TaskItem TaskItem { get; private set; }

    private TaskNote()
    {
    }
    
    public TaskNote(Guid taskItemId, string note)
    {
        TaskItemId = taskItemId;
        Note = note;
    }
}