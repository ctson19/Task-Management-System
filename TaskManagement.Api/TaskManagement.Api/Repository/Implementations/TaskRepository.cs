using Microsoft.EntityFrameworkCore;
using TaskManagement.Api.Models;
using TaskManagement.Api.Repository.Interfaces;

namespace TaskManagement.Api.Repository.Implementations
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagementDbContext _context;

        public TaskRepository(TaskManagementDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TaskItem task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(Guid taskId)
        {
            return await _context.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);
        }

        public async Task<List<TaskItem>> GetByProjectAsync(Guid projectId)
        {
            return await _context.Tasks
                .Where(x => x.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task UpdateAsync(TaskItem task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TaskItem task)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    
    }
}
