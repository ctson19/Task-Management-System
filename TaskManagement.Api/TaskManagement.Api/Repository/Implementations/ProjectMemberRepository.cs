using Microsoft.EntityFrameworkCore;
using TaskManagement.Api.Models;
using TaskManagement.Api.Repository.Interfaces;

namespace TaskManagement.Api.Repository.Implementations
{
    public class ProjectMemberRepository : IProjectMemberRepository
    {
        private readonly TaskManagementDbContext _context;

        public ProjectMemberRepository(TaskManagementDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsManagerAsync(Guid projectId, Guid userId)
        {
            return await _context.ProjectMembers.AnyAsync(x =>
                x.ProjectId == projectId &&
                x.UserId == userId &&
                (x.Role == "Owner" || x.Role == "Manager"));
        }

        public async Task<bool> ExistsAsync(Guid projectId, Guid userId)
        {
            return await _context.ProjectMembers
                .AnyAsync(x => x.ProjectId == projectId && x.UserId == userId);
        }

        public async Task AddAsync(ProjectMember member)
        {
            await _context.ProjectMembers.AddAsync(member);
        }

        public async Task<List<ProjectMember>> GetByProjectAsync(Guid projectId)
        {
            return await _context.ProjectMembers
                .Include(x => x.User)
                .Where(x => x.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<ProjectMember?> GetAsync(Guid projectId, Guid userId)
        {
            return await _context.ProjectMembers
                .FirstOrDefaultAsync(x => x.ProjectId == projectId && x.UserId == userId);
        }

        public async Task RemoveAsync(ProjectMember member)
        {
            _context.ProjectMembers.Remove(member);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}
