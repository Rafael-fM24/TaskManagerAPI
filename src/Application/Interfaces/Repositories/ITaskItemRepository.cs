using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.Repositories;

public interface ITaskItemRepository
{
    Task<TaskItem?> GetByIdAsync(Guid id);
    
    Task<IReadOnlyList<TaskItem>> GetByUserIdAsync(Guid userId, int pageNumber, int pageQuantity);
    
    void Add(TaskItem taskItem);
    
    void Remove(TaskItem taskItem);
    
    Task SaveAsync();
}