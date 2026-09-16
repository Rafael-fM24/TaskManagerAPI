using Domain.Entities;

namespace Domain.UnitTest.Entities;

public class TaskNoteTests
{
    private readonly TaskNote _taskNote = new (
        Guid.NewGuid(),
        "Note"
        );
    
    [Fact]
    public void Constructor_ShouldCreateNoteAsNotDone()
    {
        // Arrange
        var taskItemId = Guid.NewGuid();
        var note = "Note";

        // Act
        var taskNote = new TaskNote(taskItemId, note);

        // Assert
        Assert.Equal(taskItemId, taskNote.TaskItemId);
        Assert.Equal(note, taskNote.Note);
        Assert.False(taskNote.Done);
    }
    
    [Fact]
    public void Update_ShouldChangeNote()
    {
        // Arrange & Act
        _taskNote.Update("New Note");

        // Assert
        Assert.Equal("New Note", _taskNote.Note);
    }
    
    [Fact]
    public void MarkAsDone_ShouldSetDoneToTrue()
    {
        // Arrange & Act
        _taskNote.MarkAsDone();

        // Assert
        Assert.True(_taskNote.Done);
    }
    
    [Fact]
    public void MarkAsUnDone_ShouldSetDoneToFalse()
    {
        // Arrange 
        var taskNote = _taskNote;

        taskNote.MarkAsDone();

        // Act
        taskNote.MarkAsUnDone();

        // Assert
        Assert.False(taskNote.Done);
    }
}