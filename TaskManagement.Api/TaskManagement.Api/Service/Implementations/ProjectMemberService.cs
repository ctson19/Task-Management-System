using TaskManagement.Api.DTO.ProjectMembers;
using TaskManagement.Api.Models;
using TaskManagement.Api.Repository.Interfaces;
using TaskManagement.Api.Service.Interfaces;

namespace TaskManagement.Api.Service.Implementations
{
    public class ProjectMemberService : IProjectMemberService
    {
        private readonly IProjectMemberRepository _repo;

        public ProjectMemberService(IProjectMemberRepository repo)
        {
            _repo = repo;
        }

        public async Task AddMemberAsync(AddProjectMemberDto dto, Guid currentUserId)
        {
            if (!await _repo.IsManagerAsync(dto.ProjectId, currentUserId))
                throw new Exception("No permission");

            if (await _repo.ExistsAsync(dto.ProjectId, dto.UserId))
                throw new Exception("User already in project");

            await _repo.AddAsync(new ProjectMember
            {
                ProjectId = dto.ProjectId,
                UserId = dto.UserId,
                Role = dto.Role
            });

            await _repo.SaveChangesAsync();
        }

        public async Task<List<ProjectMemberDto>> GetMembersAsync(Guid projectId)
        {
            var members = await _repo.GetByProjectAsync(projectId);

            return members.Select(x => new ProjectMemberDto
            {
                UserId = x.UserId,
                UserName = x.User.UserName,
                Role = x.Role,
                JoinedAt = x.JoinedAt
            }).ToList();
        }

        public async Task UpdateRoleAsync(Guid projectId, Guid userId, string role, Guid currentUserId)
        {
            if (!await _repo.IsManagerAsync(projectId, currentUserId))
                throw new Exception("No permission");

            var member = await _repo.GetAsync(projectId, userId)
                ?? throw new Exception("Member not found");

            member.Role = role;
            await _repo.SaveChangesAsync();
        }

        public async Task RemoveMemberAsync(Guid projectId, Guid userId, Guid currentUserId)
        {
            if (!await _repo.IsManagerAsync(projectId, currentUserId))
                throw new Exception("No permission");

            var member = await _repo.GetAsync(projectId, userId)
                ?? throw new Exception("Member not found");

            await _repo.RemoveAsync(member);
            await _repo.SaveChangesAsync();
        }
    }
}
