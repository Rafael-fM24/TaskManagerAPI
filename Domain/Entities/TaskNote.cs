namespace Domain.Entities;

public class TaskNote 
{
    public int Id { get; private set; }
    
    public Guid TaskItemId { get; private set; }
    
    public string Note { get; private set; }

    private TaskNote()
    {
    }
    
    public TaskNote(Guid taskItemId, string note)
    {
        TaskItemId = taskItemId;
        Note = note;
    }
}