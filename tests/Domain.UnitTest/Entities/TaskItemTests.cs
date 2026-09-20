using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;

namespace Domain.UnitTest.Entities;

public class TaskItemTests
{
    private readonly TaskItem _taskItem = new(
            Guid.NewGuid(),
            "Título original",
            "Descrição original",
            new DateTime(2026, 9, 10),
            PriorityLevel.Low);
    
    [Fact]
    public void Constructor_ShouldInitializeTaskProperties()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var title = "Título";
        var description = "Descrição";
        var dueDate = new DateTime(2026, 10, 20);
        var priority = PriorityLevel.High;

        // Act
        var task = new TaskItem(
            userId,
            title,
            description,
            dueDate,
            priority);

        // Assert
        Assert.Equal(userId, task.UserId);
        Assert.Equal(title, task.Title);
        Assert.Equal(description, task.Description);
        Assert.Equal(Status.Pending, task.Status);
        Assert.Equal(dueDate.Date, task.DueDate);
        Assert.Equal(priority, task.Priority);
    }
    
    [Fact]
    public void Constructor_ShouldThrowDomainException_WhenPriorityIsInvalid()
    {
        // Arrange
        var invalidPriority = (PriorityLevel)999;

        // Act
        var action = () => new TaskItem(
            Guid.NewGuid(),
            "Título",
            "Descrição",
            null,
            invalidPriority);

        // Assert
        Assert.Throws<DomainException>(action);
    }
    
    [Fact]
    public void Update_ShouldUpdateTaskProperties()
    {
        // Arrange
        var newTitle = "Novo título";
        var newDescription = "Nova descrição";
        var newDueDate = new DateTime(2026, 10, 20, 15, 30, 0);
        var newPriority = PriorityLevel.High;
        
        // Act
        _taskItem.Update(newTitle, 
            newDescription, 
            newDueDate, 
            newPriority);
        
        // Assert
        Assert.Equal(newTitle, _taskItem.Title);
        Assert.Equal(newDescription, _taskItem.Description);
        Assert.Equal(newDueDate.Date, _taskItem.DueDate);
        Assert.Equal(newPriority, _taskItem.Priority);
    }
    
    [Fact]
    public void Update_ShouldSetDueDateToNull()
    {
        // Act
        _taskItem.Update(
            _taskItem.Title, 
            _taskItem.Description, 
            null, 
            _taskItem.Priority);
        
        // Assert
        Assert.Null(_taskItem.DueDate);
    }
    
    [Fact]
    public void Update_ShouldThrowDomainException_WhenPriorityIsInvalid()
    {
        // Arrange
        var invalidPriority = (PriorityLevel)999;

        // Act
        var action = () => _taskItem.Update(
            _taskItem.Title,
            _taskItem.Description,
            _taskItem.DueDate,
            invalidPriority);

        // Assert
        Assert.Throws<DomainException>(action);
    }

    [Fact]
    public void AddNote_ShouldSetNoteProperties()
    {
        // Arrange
        var noteText = "NewNote";

        // Act
        var taskNote = _taskItem.AddNote(noteText);

        // Assert
        Assert.NotNull(taskNote);
        Assert.Equal(noteText, taskNote.Note);
    }

    [Fact]
    public void UpdateNote_ShouldUpdateNoteText()
    {
        // Arrange
        var currentNote = "Current Note";
        
        var taskNote = _taskItem.AddNote(currentNote);
        
        var updateNoteText = "Update Note";
        
        // Act
        _taskItem.UpdateNote(taskNote.Id,updateNoteText);
        
        // Assert
        Assert.Equal(updateNoteText, taskNote.Note);
    }

    [Fact]
    public void UpdateNote_ShouldThrowNotFoundException_WhenNoteDoesNotExist()
    {
        // Arrange
        var invalidNoteId = 999;

        // Act
        var action = () =>
            _taskItem.UpdateNote(invalidNoteId, "Updated Note");

        // Assert
        Assert.Throws<NotFoundException>(action);
    }

    [Fact]
    public void RemoveNote_ShouldRemoveNote()
    {
        // Arrange
        var taskNote = _taskItem.AddNote("Note to remove");

        // Act
        _taskItem.RemoveNote(taskNote.Id);

        // Assert
        Assert.DoesNotContain(taskNote, _taskItem.Notes);
    }
    
    [Fact]
    public void RemoveNote_ShouldThrowNotFoundException_WhenNoteDoesNotExist()
    {
        // Arrange
        var invalidNoteId = 999;

        // Act
        var action = () => _taskItem.RemoveNote(invalidNoteId);

        // Assert
        Assert.Throws<NotFoundException>(action);
    }
    
    [Fact]
    public void InProgress_ShouldSetStatusToInProgress()
    {
        // Act
        _taskItem.InProgress();
        
        // Assert
        Assert.Equal(Status.InProgress, _taskItem.Status);
    }

    [Fact]
    public void Complete_ShouldSetStatusToCompleted()
    {
        // Arrange
        _taskItem.InProgress();
        
        // Act
        _taskItem.Complete();
        
        // Assert
        Assert.Equal(Status.Completed, _taskItem.Status);
    }

    [Fact]
    public void Complete_ShouldThrowDomainException_WhenStatusIsPending()
    {
        // Act
        var action = () => _taskItem.Complete();
        
        //Assert
        Assert.Throws<DomainException>(action);
    }

    [Fact]
    public void Complete_ShouldThrowDomainException_WhenThereArePendingNotes()
    {
        // Arrange
        _taskItem.InProgress();

        _taskItem.AddNote("Note");
        
        // Act
        var action = () => _taskItem.Complete();
        
        // Assert
        Assert.Throws<DomainException>(action);
    }

    [Fact]
    public void MarkAsDone_ShouldSetDoneStatus()
    {
        // Arrange
        var taskNote = _taskItem.AddNote("Note");
        
        // Act
        _taskItem.MarkNoteAsDone(taskNote.Id);
        
        // Assert
        Assert.True(taskNote.Done);
    }
    
    [Fact]
    public void MarkAsUndone_ShouldSetUndoneStatus()
    {
        // Arrange
        var taskNote = _taskItem.AddNote("Note");
        
        // Act
        _taskItem.MarkNoteAsUndone(taskNote.Id);
        
        // Assert
        Assert.False(taskNote.Done);
    }
}