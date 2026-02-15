using Microsoft.EntityFrameworkCore;
using TaskManagement.Api.Models;
using TaskManagement.Api.Repository.Interfaces;

namespace TaskManagement.Api.Repository.Implementations
{
    public class TaskCommentRepository : ITaskCommentRepository
    {
        private readonly TaskManagementDbContext _context;

        public TaskCommentRepository(TaskManagementDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TaskComment comment)
        {
            await _context.TaskComments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TaskComment>> GetByTaskAsync(Guid taskId)
        {
            return await _context.TaskComments
                .Include(c => c.User)
                .Where(c => c.TaskId == taskId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<TaskComment?> GetByIdAsync(Guid id)
        {
            return await _context.TaskComments
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateAsync(TaskComment comment)
        {
            _context.TaskComments.Update(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TaskComment comment)
        {
            _context.TaskComments.Remove(comment);
            await _context.SaveChangesAsync();
        }
    }
}


