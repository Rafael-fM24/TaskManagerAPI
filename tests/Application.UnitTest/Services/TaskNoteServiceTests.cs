using Application.DTOs.TaskNote;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Moq;

namespace Application.UnitTest.Services;

public class TaskNoteServiceTests
{
    private readonly Mock<ICurrentUserService> _currentUserServiceMock = new ();
    private readonly Mock<ITaskNoteRepository> _taskNoteRepositoryMock = new ();
    private readonly Mock<ITaskItemRepository> _taskItemRepositoryMock = new ();
    private readonly Mock<IMapper> _mapperMock = new ();
    private readonly List<TaskNote> _taskNotes;
        
    private readonly TaskItem _taskItem = new(
        Guid.NewGuid(),
        "Task",
        "Description",
        null,
        PriorityLevel.None
    );
    
    public TaskNoteServiceTests()
    {
        _taskNotes =
        [
            new TaskNote(_taskItem.Id, "Note1"),
            
            new TaskNote(_taskItem.Id, "Note2")
        ];
    }

    private TaskNoteService CreateTaskNoteService()
    {
        return new TaskNoteService(
            _taskNoteRepositoryMock.Object,
            _taskItemRepositoryMock.Object,
            _currentUserServiceMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnNotes()
    {
        // Arrange
        var taskNoteDTO = new List<TaskNoteDTO>
        {
            new(),
            new()
        };
        
        _taskNoteRepositoryMock
            .Setup(x => x.GetAllNotesAsync(_taskItem.Id, 0, 5))
            .ReturnsAsync(_taskNotes);
        
        _mapperMock
            .Setup(x => x.Map<IReadOnlyList<TaskNoteDTO>>(_taskNotes))
            .Returns(taskNoteDTO);

        var service = CreateTaskNoteService();
        
        // Act
        await service.GetAllAsync(_taskItem.Id, 0, 5);
        
        // Assert
        _taskNoteRepositoryMock.Verify(
                x => x.GetAllNotesAsync(_taskItem.Id, 0, 5), 
                Times.Once);
        
        _mapperMock.Verify(
            x => x.Map<IReadOnlyList<TaskNoteDTO>>(_taskNotes),
            Times.Once);
    }
    
    [Fact]
    public async Task CreateAsync_ShouldCreateNote()
    {
        // Assert
        var dto = new CreateTaskNoteDTO
        {
            Note = "Note"
        };

        _currentUserServiceMock
            .Setup(x => x.UserId)
            .Returns(_taskItem.UserId);
        
        _taskItemRepositoryMock
            .Setup(x => x.GetByIdAsync(_taskItem.Id, _taskItem.UserId))
            .ReturnsAsync(_taskItem);
        
        var service = CreateTaskNoteService();
        
        // Act 
        await service.CreateAsync(_taskItem.Id, dto);
        
        // Arrange
        _taskNoteRepositoryMock.Verify(
            x => x.Add(It.Is<TaskNote>(
                taskNote => 
                    taskNote.TaskItemId == _taskItem.Id &&
                    taskNote.Note == dto.Note
            )),
            Times.Once
        );
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowNotFoundException_WhenTaskDoesNotExist()
    {
        // Assert
        var taskItemId = Guid.NewGuid();
        
        _currentUserServiceMock
            .Setup(x => x.UserId)
            .Returns(_taskItem.UserId);
        
        _taskItemRepositoryMock
            .Setup(x => x.GetByIdAsync(taskItemId, _taskItem.UserId))
            .ReturnsAsync((TaskItem?)null);
        
        var service = CreateTaskNoteService();

        // Act & Arrange
        await Assert.ThrowsAsync<NotFoundException>( 
            () => service.CreateAsync(taskItemId,  new CreateTaskNoteDTO
            {
                Note = "Note"
            }));
        
        _taskNoteRepositoryMock.Verify(
            x => x.SaveAsync(), 
            Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateNote()
    {
        // Arrange
        var dto = new UpdateTaskNoteDTO
        {
            Note = "NoteUpdate"
        };
        
        _taskNoteRepositoryMock
            .Setup(x => x.GetByIdAsync(_taskNotes[0].Id))
            .ReturnsAsync(_taskNotes[0]);
        
        var service = CreateTaskNoteService();
        
        // Act 
        await service.UpdateAsync(_taskNotes[0].Id, dto);
        
        // Assert
        Assert.Equal(dto.Note, _taskNotes[0].Note);
        
        _taskNoteRepositoryMock.Verify(
            x => x.SaveAsync(), 
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowNotFoundException_WhenNoteDoesNotExist()
    {
        // Arrange
        var taskNoteId = 9;
        
        _taskNoteRepositoryMock
            .Setup(x => x.GetByIdAsync(taskNoteId))
            .ReturnsAsync((TaskNote?)null);
        
        var service = CreateTaskNoteService();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => service.UpdateAsync(taskNoteId, new UpdateTaskNoteDTO
            {
                Note = "NoteUpdate"
            }));
        
        _taskNoteRepositoryMock.Verify(
            x => x.SaveAsync(), 
            Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteNote()
    {
        // Arrange
        _taskNoteRepositoryMock
            .Setup(x => x.GetByIdAsync(_taskNotes[1].Id))
            .ReturnsAsync(_taskNotes[1]);
        
        var service = CreateTaskNoteService();
        
        // Act 
        await service.DeleteAsync(_taskNotes[1].Id);
        
        // Arrange
        _taskNoteRepositoryMock.Verify(
            x => x.Remove(_taskNotes[1]), 
            Times.Once);
        
        _taskNoteRepositoryMock.Verify(
            x => x.SaveAsync(), 
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowNotFoundException_WhenNoteDoesNotExist()
    {
        // Arrange
        var taskNoteId = 9;
        
        _taskNoteRepositoryMock
            .Setup(x => x.GetByIdAsync(taskNoteId))
            .ReturnsAsync((TaskNote?)null);
        
        var service = CreateTaskNoteService();
        
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => service.DeleteAsync(taskNoteId));
        
        _taskNoteRepositoryMock.Verify(
            x => x.SaveAsync(), 
            Times.Never);
    }
}