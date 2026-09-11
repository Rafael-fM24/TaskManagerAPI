using Application.DTOs.TaskItem;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Moq;

namespace Application.UnitTest.Services;

public class TaskItemServiceTests
{
    private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
    private readonly Mock<ITaskItemRepository> _taskItemRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Guid _userId = Guid.NewGuid();
    private readonly List<TaskItem> _taskItems;

    public TaskItemServiceTests()
    {
        _currentUserServiceMock
            .Setup(x => x.UserId)
            .Returns(_userId);
        
        _taskItems =
        [
            new TaskItem(
                _userId,
                "Título1",
                "Descrição1",
                new DateTime(2026, 9, 10),
                PriorityLevel.Low),

            new TaskItem(
                _userId,
                "Título2",
                "Descrição2",
                null,
                PriorityLevel.High)
        ];
    }
    
    private TaskItemService CreateService()
    {
        return new TaskItemService(
            _taskItemRepositoryMock.Object,
            _mapperMock.Object,
            _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task GetAllTasksAsync_ShouldReturnUserTasks()
    {
        // Arrange
        var taskItemsDTO = new List<TaskItemDTO>
        {
            new(),
            new()
        };

        _taskItemRepositoryMock
            .Setup(x => x.GetByUserIdAsync(_userId, 0, 5))
            .ReturnsAsync(_taskItems);

        _mapperMock
            .Setup(x => x.Map<IReadOnlyList<TaskItemDTO>>(_taskItems))
            .Returns(taskItemsDTO);

        var service = CreateService();

        // Act
        var result = await service.GetAllTasksAsync(0, 5);

        // Assert
        _taskItemRepositoryMock.Verify(
            x => x.GetByUserIdAsync(_userId, 0, 5),
            Times.Once);
        
        Assert.NotNull(result);
        Assert.Equal(taskItemsDTO, result);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateTask()
    {
        // Arrange
        var dto = new CreateTaskItemDTO
        {
            Title = "Title",
            Description = "Description",
            DueDate = null,
            Priority = PriorityLevel.None
        };

        // Act
        var service = CreateService();
        await service.CreateAsync(dto);

        // Assert
        _taskItemRepositoryMock.Verify(
            x => x.Add(It.Is<TaskItem>(task =>
                task.UserId == _userId &&
                task.Title == dto.Title &&
                task.Description == dto.Description &&
                task.DueDate == dto.DueDate &&
                task.Priority == dto.Priority
            )),
            Times.Once);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldUpdateTask()
    {
        // Arrange
        var dto = new UpdateTaskItemDTO
        {
            Title = "Title",
            Description = _taskItems[0].Description,
            DueDate = _taskItems[0].DueDate,
            Priority = _taskItems[0].Priority
        };
        
        _taskItemRepositoryMock
            .Setup(x => x.GetByIdAsync(_taskItems[0].Id, _userId))
            .ReturnsAsync(_taskItems[0]);

        var service = CreateService();

       // Act
       await service.UpdateAsync(_taskItems[0].Id, dto);
       
       // Assert
       _taskItemRepositoryMock.Verify(
           x => x.SaveAsync(),
           Times.Once);
       
       Assert.Equal(dto.Title, _taskItems[0].Title);
       Assert.Equal(dto.Description, _taskItems[0].Description);
       Assert.Equal(dto.DueDate, _taskItems[0].DueDate);
       Assert.Equal(dto.Priority, _taskItems[0].Priority);
    }

    [Fact]
    public async Task UpdateAsync_WhenTaskNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var taskItemId = Guid.NewGuid();

        _taskItemRepositoryMock
            .Setup(x => x.GetByIdAsync(taskItemId, _userId))
            .ReturnsAsync((TaskItem?)null);

        var service = CreateService();

        // Act and Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => service.UpdateAsync(taskItemId, new UpdateTaskItemDTO
            {
                Title = "Title",
                Description = "Description",
                DueDate = null,
                Priority = PriorityLevel.Low
            }));
        
        _taskItemRepositoryMock.Verify(
            x => x.SaveAsync(),
            Times.Never);
    }

    [Theory]
    [InlineData(PriorityLevel.None)]
    [InlineData(PriorityLevel.Low)]
    [InlineData(PriorityLevel.Medium)]
    [InlineData(PriorityLevel.High)]
    public async Task UpdateAsync_ShouldUpdatePriority(PriorityLevel priority)
    {
        // Arrange
        var dto = new UpdateTaskItemDTO
        {
            Title = _taskItems[1].Title,
            Description = _taskItems[1].Description,
            DueDate = _taskItems[1].DueDate,
            Priority = priority
        };
        
        _taskItemRepositoryMock
            .Setup(x => x.GetByIdAsync(_taskItems[1].Id, _userId))
            .ReturnsAsync(_taskItems[1]);

        var service = CreateService();
        
        // Act 
        await service.UpdateAsync(_taskItems[1].Id, dto);
        
        // Assert
        _taskItemRepositoryMock.Verify(
            x => x.SaveAsync(),
            Times.Once);
        
        Assert.Equal(dto.Priority, _taskItems[1].Priority);
    }

    [Fact]
    public async Task CompleteAsync_ShouldCompleteTask()
    {
        // Arrange
        _taskItemRepositoryMock
            .Setup(x => x.GetByIdAsync(_taskItems[1].Id, _userId))
            .ReturnsAsync(_taskItems[1]);

        var service = CreateService();
        
        //Act
        await service.CompletedAsync(_taskItems[1].Id);
        
        // Assert
        _taskItemRepositoryMock.Verify(
            x => x.SaveAsync(),
            Times.Once);
        
        Assert.True(_taskItems[1].Completed);
    }

    [Fact]
    public async Task CompleteAsync_WhenTaskNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var taskItemId = Guid.NewGuid();
        
        _taskItemRepositoryMock
            .Setup(x => x.GetByIdAsync(taskItemId, _userId))
            .ReturnsAsync((TaskItem?)null);

        var service = CreateService();
        
        //Act && Assert
        await Assert.ThrowsAsync<NotFoundException>(
        () => service.CompletedAsync(taskItemId));
        
        _taskItemRepositoryMock.Verify(
            x => x.SaveAsync(),
            Times.Never);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldDeleteTask()
    {
        // Arrange
        _taskItemRepositoryMock
            .Setup(x => x.GetByIdAsync(_taskItems[1].Id, _userId))
            .ReturnsAsync(_taskItems[1]);

        var service = CreateService();
        
        // Act
        
        await service.DeleteAsync(_taskItems[1].Id);

        // Assert
        _taskItemRepositoryMock.Verify(
            x => x.Remove(_taskItems[1]),
            Times.Once);

        _taskItemRepositoryMock.Verify(
            x => x.SaveAsync(),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenTaskNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var taskItemId = Guid.NewGuid();
        
        _taskItemRepositoryMock
            .Setup(x => x.GetByIdAsync(taskItemId, _userId))
            .ReturnsAsync((TaskItem?)null);

        var service = CreateService();
        
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => service.DeleteAsync(taskItemId));

        _taskItemRepositoryMock.Verify(
            x => x.Remove(It.IsAny<TaskItem>()),
            Times.Never);
        
        _taskItemRepositoryMock.Verify(
            x => x.SaveAsync(),
            Times.Never);
    }
}