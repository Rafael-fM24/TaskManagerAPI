using Domain.Exceptions;

public class TaskNote
{
    public int Id { get; private set; }

    public bool Done { get; private set; }

    public string Note { get; private set; }

    private TaskNote()
    {
    }
    
    private static void ValidateNote(string note)
    {
        if (string.IsNullOrWhiteSpace(note))
            throw new DomainException("A nota não pode ser vazia.");
    }

    internal TaskNote(string note)
    {
        ValidateNote(note);
        
        Note = note;
        Done = false;
    }

    internal void Update(string note)
    {
        ValidateNote(note);
        
        Note = note;
    }

    internal void MarkAsDone()
    {
        Done = true;
    }

    internal void MarkAsUndone()
    {
        Done = false;
    }
    
    
}