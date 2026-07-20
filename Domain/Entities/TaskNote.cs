using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("TaskNote")]
public class TaskNote 
{
    [Key]
    public int Id { get; set; }
    public Guid TaskItemId { get; set; }
    public string Note { get; set; }
    
    [ForeignKey(nameof(TaskItemId))]
    public virtual TaskItem TaskItem { get; set; }

    public TaskNote(Guid taskItemId, string note)
    {
        TaskItemId = taskItemId;
        Note = note;
    }
}