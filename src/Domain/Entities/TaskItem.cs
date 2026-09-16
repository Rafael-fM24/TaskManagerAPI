using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Entities;

public class TaskItem
{
    public Guid Id { get; private set; }
    
    public Guid UserId { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;
    
    public DateTime Created { get; private set; }
    
    public DateTime? DueDate { get; private set; }
    
    public Status Status {get; private set;}
    
    public PriorityLevel Priority { get; private set; }
    
    public ICollection<TaskNote> Notes { get; private set; } = new List<TaskNote>(); 

    private TaskItem()
    {
    }
    
    private static void ValidatePriority(PriorityLevel priority)
    {
        if (!Enum.IsDefined(priority))
            throw new DomainException("Prioridade inválida.");
    }
    
    public TaskItem(Guid userId,
        string title, 
        string description, 
        DateTime? dueDate, 
        PriorityLevel priority)
    {
        ValidatePriority(priority);
        
        Id = Guid.NewGuid();
        UserId = userId;
        Title = title;
        Description = description;
        Created = DateTime.UtcNow;
        DueDate = dueDate?.Date;
        Status = Status.Pending;
        Priority = priority;
    }
    
    public void Update(
        string title,
        string description,
        DateTime? dueDate,
        PriorityLevel priority)
    {
        ValidatePriority(priority);
        
        Title = title;
        Description = description;
        DueDate = dueDate?.Date;
        Priority = priority;
    }

    public void InProgress()
    {
        if (Status != Status.InProgress)
            Status = Status.InProgress;
    }
    
    public void Complete()
    {
        if (Status == Status.Pending)
            throw new DomainException("A pending task cannot be completed.");

        if (Status != Status.Completed)
            Status = Status.Completed;
    }
    
    public void CompleteFromNotes()
    {
        Status = Status.Completed;
    }
}