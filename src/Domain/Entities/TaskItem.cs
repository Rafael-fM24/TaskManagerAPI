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
    
    private readonly List<TaskNote> _notes = new();

    public IReadOnlyCollection<TaskNote> Notes => _notes.AsReadOnly();

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
            throw new DomainException(
                "A pending task cannot be completed.");

        if (_notes.Any(n => !n.Done))
            throw new DomainException(
                "All task notes must be completed.");

        Status = Status.Completed;
    }
    
    public TaskNote AddNote(string note)
    {
        var taskNote = new TaskNote(note);

        _notes.Add(taskNote);

        return taskNote;
    }

    public void UpdateNote(int noteId, string note)
    {
        var taskNote = GetNote(noteId);

        taskNote.Update(note);
    }

    public void RemoveNote(int noteId)
    {
        var taskNote = GetNote(noteId);

        _notes.Remove(taskNote);
    }

    public void MarkNoteAsDone(int noteId)
    {
        var taskNote = GetNote(noteId);

        taskNote.MarkAsDone();
    }

    public void MarkNoteAsUndone(int noteId)
    {
        var taskNote = GetNote(noteId);

        taskNote.MarkAsUndone();
    }
    
    public bool AllNotesDone(int currentNoteId)
    {
        return Notes
            .Where(n => n.Id != currentNoteId)
            .All(n => n.Done);
    }

    private TaskNote GetNote(int noteId)
    {
        var note = _notes.FirstOrDefault(n => n.Id == noteId);

        if (note == null)
            throw new NotFoundException("TaskNote not found.");

        return note;
    }
}