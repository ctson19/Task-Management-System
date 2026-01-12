using TaskManagement.Api.Models;

namespace TaskManagement.Api.Repository.Interfaces
{
    public interface IProjectMemberRepository
    {
        Task<bool> IsManagerAsync(Guid projectId, Guid userId);
        Task<bool> ExistsAsync(Guid projectId, Guid userId);

        Task AddAsync(ProjectMember member);
        Task<List<ProjectMember>> GetByProjectAsync(Guid projectId);

        Task<ProjectMember?> GetAsync(Guid projectId, Guid userId);
        Task RemoveAsync(ProjectMember member);

        Task SaveChangesAsync();
    }
}
