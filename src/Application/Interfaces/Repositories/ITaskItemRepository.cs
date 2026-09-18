using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ITaskItemRepository
{
    Task<TaskItem?> GetByIdAsync(Guid id, Guid userId);
    
    Task<IReadOnlyList<TaskItem>> GetByUserIdAsync(Guid userId, int pageNumber, int pageQuantity);
    
    Task<IReadOnlyList<TaskNote>> GetNotesAsync(
        Guid taskItemId,
        Guid userId,
        int pageNumber,
        int pageQuantity);
    
    void Add(TaskItem taskItem);
    
    void Remove(TaskItem taskItem);
    
    Task SaveAsync();
}