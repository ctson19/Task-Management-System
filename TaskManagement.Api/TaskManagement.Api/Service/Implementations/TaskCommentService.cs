using TaskManagement.Api.DTO.TaskComments;
using TaskManagement.Api.Models;
using TaskManagement.Api.Repository.Interfaces;
using TaskManagement.Api.Service.Interfaces;

namespace TaskManagement.Api.Service.Implementations
{
    public class TaskCommentService : ITaskCommentService
    {
        private readonly ITaskCommentRepository _commentRepo;
        private readonly ITaskRepository _taskRepo;
        private readonly IProjectMemberRepository _memberRepo;

        public TaskCommentService(
            ITaskCommentRepository commentRepo,
            ITaskRepository taskRepo,
            IProjectMemberRepository memberRepo)
        {
            _commentRepo = commentRepo;
            _taskRepo = taskRepo;
            _memberRepo = memberRepo;
        }

        public async Task<List<TaskCommentResponseDto>> GetByTaskAsync(Guid taskId, Guid currentUserId)
        {
            var task = await _taskRepo.GetByIdAsync(taskId)
                ?? throw new Exception("Task not found");

            if (!await _memberRepo.ExistsAsync(task.ProjectId, currentUserId))
                throw new Exception("No permission");

            var comments = await _commentRepo.GetByTaskAsync(taskId);
            return comments.Select(MapToDto).ToList();
        }

        public async Task<TaskCommentResponseDto> CreateAsync(Guid taskId, CreateTaskCommentRequestDto dto, Guid currentUserId)
        {
            var task = await _taskRepo.GetByIdAsync(taskId)
                ?? throw new Exception("Task not found");

            if (!await _memberRepo.ExistsAsync(task.ProjectId, currentUserId))
                throw new Exception("No permission");

            var comment = new TaskComment
            {
                Id = Guid.NewGuid(),
                TaskId = taskId,
                UserId = currentUserId,
                Content = dto.Content,
                CreatedAt = DateTime.UtcNow
            };

            await _commentRepo.AddAsync(comment);

            // reload with user navigation
            var saved = await _commentRepo.GetByIdAsync(comment.Id) ?? comment;
            return MapToDto(saved);
        }

        public async Task UpdateAsync(Guid commentId, string content, Guid currentUserId)
        {
            var comment = await _commentRepo.GetByIdAsync(commentId)
                ?? throw new Exception("Comment not found");

            var task = await _taskRepo.GetByIdAsync(comment.TaskId)
                ?? throw new Exception("Task not found");

            var isManager = await _memberRepo.IsManagerAsync(task.ProjectId, currentUserId);
            var isOwner = comment.UserId == currentUserId;

            if (!isManager && !isOwner)
                throw new Exception("No permission");

            comment.Content = content;
            await _commentRepo.UpdateAsync(comment);
        }

        public async Task DeleteAsync(Guid commentId, Guid currentUserId)
        {
            var comment = await _commentRepo.GetByIdAsync(commentId)
                ?? throw new Exception("Comment not found");

            var task = await _taskRepo.GetByIdAsync(comment.TaskId)
                ?? throw new Exception("Task not found");

            var isManager = await _memberRepo.IsManagerAsync(task.ProjectId, currentUserId);
            var isOwner = comment.UserId == currentUserId;

            if (!isManager && !isOwner)
                throw new Exception("No permission");

            await _commentRepo.DeleteAsync(comment);
        }

        private static TaskCommentResponseDto MapToDto(TaskComment comment)
        {
            return new TaskCommentResponseDto
            {
                Id = comment.Id,
                TaskId = comment.TaskId,
                UserId = comment.UserId,
                UserName = comment.User?.UserName ?? string.Empty,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt
            };
        }
    }
}


