using Domain.Enums;
using Domain.Exceptions;

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
        DateTime dueDate, 
        PriorityLevel priority)
    {
        ValidatePriority(priority);
        
        Id = Guid.NewGuid();
        UserId = userId;
        Title = title;
        Description = description;
        Completed = false;
        Created = DateTime.UtcNow;
        DueDate = dueDate.Date;
        Priority = priority;
    }
    
    public void Update(
        string title,
        string description,
        DateTime dueDate,
        PriorityLevel priority)
    {
        ValidatePriority(priority);
        
        Title = title;
        Description = description;
        DueDate = dueDate.Date;
        Priority = priority;
    }
    
    public void Complete()
    {
        Completed = true;
    }
}