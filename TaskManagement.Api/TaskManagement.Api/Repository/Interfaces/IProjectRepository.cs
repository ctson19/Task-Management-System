using TaskManagement.Api.Models;

namespace TaskManagement.Api.Repository.Interfaces
{
    public interface IProjectRepository
    {
        Task AddAsync(Project project);
        Task<Project?> GetByIdAsync(Guid id);
        Task<List<Project>> GetByUserAsync(Guid userId);
        Task UpdateAsync(Project project);
        Task DeleteAsync(Project project);
    }
}
