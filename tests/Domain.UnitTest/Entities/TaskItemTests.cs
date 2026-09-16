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
        Assert.False(task.Status == Status.Pending);
        Assert.Equal(dueDate.Date, task.DueDate);
        Assert.Equal(priority, task.Priority);
    }

    [Fact]
    public void Constructor_ShouldSetDueDateToNull_WhenDueDateIsNull()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var title = "Título";
        var description = "Descrição";
        DateTime? dueDate = null;
        var priority = PriorityLevel.High;

        // Act
        var task = new TaskItem(
            userId,
            title,
            description,
            dueDate?.Date,
            priority);

        // Assert
        Assert.Equal(userId, task.UserId);
        Assert.Equal(title, task.Title);
        Assert.Equal(description, task.Description);
        Assert.False(task.Status == Status.Pending);
        Assert.Null(task.DueDate);
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
    public void Complete_ShouldMarkTaskAsCompleted()
    {
        // Act
        _taskItem.Complete();
        
        // Assert
        Assert.True(_taskItem.Status == Status.Completed);
    }
}