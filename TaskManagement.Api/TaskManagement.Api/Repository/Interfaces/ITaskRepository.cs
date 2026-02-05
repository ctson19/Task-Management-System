using TaskManagement.Api.Models;

namespace TaskManagement.Api.Repository.Interfaces
{
    public interface ITaskRepository
    {
        Task AddAsync(TaskItem task);
        Task<TaskItem?> GetByIdAsync(Guid taskId);
        Task<List<TaskItem>> GetByProjectAsync(Guid projectId);
        Task UpdateAsync(TaskItem task);
        Task DeleteAsync(TaskItem task);
    }
}
