namespace Application.DTOs.TaskNote;

public class TaskNoteDTO
{
    public int Id { get; set; }
    public bool Done { get; set; }
    public string Note { get; init; } = string.Empty;
}