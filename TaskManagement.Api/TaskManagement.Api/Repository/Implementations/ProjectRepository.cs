using Microsoft.EntityFrameworkCore;
using TaskManagement.Api.Models;
using TaskManagement.Api.Repository.Interfaces;

namespace TaskManagement.Api.Repository.Implementations
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly TaskManagementDbContext _context;

        public ProjectRepository(TaskManagementDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
        }

        public async Task<Project?> GetByIdAsync(Guid id)
        {
            return await _context.Projects.FindAsync(id);
        }

        public async Task<List<Project>> GetByUserAsync(Guid userId)
        {
            return await _context.Projects
                .Where(p => p.CreatedBy == userId)
                .ToListAsync();
        }

        public async Task UpdateAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Project project)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }
    }
}
