using Application.DTOs.TaskNote;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Moq;

namespace Application.UnitTest.Services;

public class TaskNoteServiceTests
{
    private readonly Mock<ITaskItemRepository> _taskItemRepositoryMock = new ();
    private readonly Mock<ICurrentUserService> _currentUserServiceMock = new ();
    private static readonly Guid UserId = Guid.NewGuid();
    private readonly TaskItem _taskItem;

    public TaskNoteServiceTests()
    {
        _currentUserServiceMock
            .Setup(x => x.UserId)
            .Returns(UserId);
        
        _taskItem = new (
            UserId,
            "Title",
            "Description",
            new DateTime(2026, 9, 10),
            PriorityLevel.Low);
    }

    private TaskNoteService CreateService()
    {
        return new TaskNoteService(_taskItemRepositoryMock.Object, _currentUserServiceMock.Object);
    }
    
    [Fact]
    public async Task CreateAsync_CreateAsync_ShouldCreateNote()
    {
        // Arrange
        var dto = new CreateTaskNoteDTO()
        {
            Note = "Note",
        };
        
        _taskItemRepositoryMock
            .Setup(x => x.GetByIdAsync(_taskItem.Id, UserId))
            .ReturnsAsync(_taskItem);

        var service = CreateService();

        // Act
        await service.CreateAsync(_taskItem.Id, dto);
        
        // Assert
        Assert.Equal(dto.Note, _taskItem.Notes.FirstOrDefault()?.Note);
        Assert.Equal(Status.InProgress, _taskItem.Status);
        
        _taskItemRepositoryMock.Verify(
            x => x.SaveAsync(), 
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowNotFoundException_WhenTaskDoesNotExist()
    {
        // Assert
        var taskItemId = Guid.NewGuid();
        
        var dto = new CreateTaskNoteDTO()
        {
            Note = "Note",
        };
        
        _taskItemRepositoryMock
            .Setup(x => x.GetByIdAsync(taskItemId, UserId))
            .ReturnsAsync((TaskItem?)null);

        var service = CreateService();
        
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => service.CreateAsync(taskItemId, dto));
        
        _taskItemRepositoryMock.Verify(
            x => x.SaveAsync(), 
            Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateNote()
    {
        // Arrange
        var taskNote = _taskItem.AddNote("Original note");
        
        var dto = new UpdateTaskNoteDTO()
        {
            Note = "Updated note"
        };
        
        _taskItemRepositoryMock
            .Setup(x => x.GetByIdAsync(_taskItem.Id, UserId))
            .ReturnsAsync(_taskItem);
        
        var service = CreateService();
        
        // Act
        await service.UpdateAsync(_taskItem.Id, taskNote.Id, dto);
        
        // Assert
        Assert.NotNull(_taskItem.Notes.FirstOrDefault()?.Note);
        Assert.Equal(dto.Note, taskNote.Note);

        _taskItemRepositoryMock.Verify(
            x => x.SaveAsync(),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowNotFoundException_WhenTaskDoesNotExist()
    {
        var taskItemId = Guid.NewGuid();
        var taskNoteId = 1;
        
        var dto = new UpdateTaskNoteDTO()
        {
            Note = "Updated note"
        };
        
        _taskItemRepositoryMock
            .Setup(x => x.GetByIdAsync(taskItemId, UserId))
            .ReturnsAsync((TaskItem?)null);
        
        var service = CreateService();
        
        await Assert.ThrowsAsync<NotFoundException>(
            () => service.UpdateAsync(taskItemId, taskNoteId, dto));
        
        _taskItemRepositoryMock.Verify(
            x => x.SaveAsync(),
            Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowNotFoundException_WhenNoteDoesNotExist()
    {
        // Arrange
        var taskNoteId = 999;

        var dto = new UpdateTaskNoteDTO
        {
            Note = "Updated note"
        };

        _taskItemRepositoryMock
            .Setup(x => x.GetByIdAsync(_taskItem.Id, UserId))
            .ReturnsAsync(_taskItem);

        var service = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => service.UpdateAsync(_taskItem.Id, taskNoteId, dto));

        _taskItemRepositoryMock.Verify(
            x => x.SaveAsync(),
            Times.Never);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldDeleteNote()
    {
        // Arrange
        var taskNote = _taskItem.AddNote("Note");
        
        _taskItemRepositoryMock
            .Setup(x => x.GetByIdAsync(_taskItem.Id, UserId))
            .ReturnsAsync(_taskItem);
        
        var service = CreateService();
        
        // Act
        await service.DeleteAsync(_taskItem.Id, taskNote.Id);
        
        // Assert
        Assert.DoesNotContain(_taskItem.Notes, x => x.Note == taskNote.Note);

        _taskItemRepositoryMock.Verify(
            x => x.SaveAsync(),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowNotFoundException_WhenNoteDoesNotExist()
    {
        // Arrange
        var taskNoteId = 1;
        
        _taskItemRepositoryMock
            .Setup(x => x.GetByIdAsync(_taskItem.Id, UserId))
            .ReturnsAsync(_taskItem);
        
        var service = CreateService();
        
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => service.DeleteAsync(_taskItem.Id, taskNoteId));

        _taskItemRepositoryMock.Verify(
            x => x.SaveAsync(),
            Times.Never);
    }

    [Fact]
    public async Task MarkAsDoneAsync_ShouldCompleteTask_WhenAllNotesAreDone()
    {
        // Arrange
        var taskNote = _taskItem.AddNote("Note");
        
        _taskItem.InProgress();
        
        _taskItemRepositoryMock
            .Setup(x => x.GetByIdAsync(_taskItem.Id, UserId))
            .ReturnsAsync(_taskItem);
        
        var service = CreateService();
        
        await service.MarkAsDoneAsync(_taskItem.Id, taskNote.Id);
        
        Assert.True(taskNote.Done);
        Assert.Equal(Status.Completed, _taskItem.Status);
        
        _taskItemRepositoryMock.Verify(
            x => x.SaveAsync(),
            Times.Once);
    }

    
    private static void SetNoteId(TaskNote note, int id)
    {
        typeof(TaskNote)
            .GetProperty(nameof(TaskNote.Id))!
            .SetValue(note, id);
    }
    
    [Fact]
    public async Task MarkAsDoneAsync_ShouldKeepTaskInProgress_WhenNotAllNotesAreDone()
    {
        // Arrange
        var firstNote = _taskItem.AddNote("First note");
        var secondNote = _taskItem.AddNote("Second note");
        
        SetNoteId(firstNote, 1);
        SetNoteId(secondNote, 2);
        
        _taskItem.InProgress();
        
        _taskItemRepositoryMock
            .Setup(x => x.GetByIdAsync(_taskItem.Id, UserId))
            .ReturnsAsync(_taskItem);
        
        var service = CreateService();
        
        await service.MarkAsDoneAsync(_taskItem.Id, firstNote.Id);
        
        
        // Assert
        Assert.True(firstNote.Done);
        Assert.False(secondNote.Done);
        Assert.Equal(Status.InProgress, _taskItem.Status);
        
        _taskItemRepositoryMock.Verify(
            x => x.SaveAsync(),
            Times.Once);
    }

    [Fact]
    public async Task MarkAsUndoneAsync_ShouldMarkNoteAsUndone_AndSetTaskInProgress()
    {
        // Arrange
        var taskNote = _taskItem.AddNote("Note");
        
        _taskItem.InProgress();
        _taskItem.MarkNoteAsDone(taskNote.Id);
        _taskItem.Complete();
        
        _taskItemRepositoryMock
            .Setup(x => x.GetByIdAsync(_taskItem.Id, UserId))
            .ReturnsAsync(_taskItem);
        
        var service = CreateService();
        
        // Act
        await service.MarkAsUndoneAsync(_taskItem.Id, taskNote.Id);

        // Assert
        Assert.False(taskNote.Done);
        Assert.Equal(Status.InProgress, _taskItem.Status);
        
        _taskItemRepositoryMock.Verify(
            x => x.SaveAsync(),
            Times.Once);
    }

    [Fact]
    public async Task MarkAsUndoneAsync_ShouldThrowNotFoundException_WhenTaskDoesNotExist()
    {
        // Arrange
        var taskItemId = Guid.NewGuid();
        var taskNoteId = 1;
        
        _taskItemRepositoryMock
            .Setup(x => x.GetByIdAsync(taskItemId, UserId))
            .ReturnsAsync((TaskItem?)null);
        
        var service = CreateService();
        
        // Act & Arrange
        await Assert.ThrowsAsync<NotFoundException>(
            () => service.MarkAsUndoneAsync(taskItemId, taskNoteId));
        
        _taskItemRepositoryMock.Verify(
            x => x.SaveAsync(),
            Times.Never);
    }
}