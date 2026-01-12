using TaskManagement.Api.DTO.ProjectMembers;

namespace TaskManagement.Api.Service.Interfaces
{
    public interface IProjectMemberService
    {
        Task AddMemberAsync(AddProjectMemberDto dto, Guid currentUserId);
        Task<List<ProjectMemberDto>> GetMembersAsync(Guid projectId);
        Task UpdateRoleAsync(Guid projectId, Guid userId, string role, Guid currentUserId);
        Task RemoveMemberAsync(Guid projectId, Guid userId, Guid currentUserId);
        
    }
}
