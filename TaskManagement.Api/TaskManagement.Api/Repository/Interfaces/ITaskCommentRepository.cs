using TaskManagement.Api.Models;

namespace TaskManagement.Api.Repository.Interfaces
{
    public interface ITaskCommentRepository
    {
        Task AddAsync(TaskComment comment);
        Task<List<TaskComment>> GetByTaskAsync(Guid taskId);
        Task<TaskComment?> GetByIdAsync(Guid id);
        Task UpdateAsync(TaskComment comment);
        Task DeleteAsync(TaskComment comment);
    }
}


