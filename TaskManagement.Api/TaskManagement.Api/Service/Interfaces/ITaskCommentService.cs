using TaskManagement.Api.DTO.TaskComments;

namespace TaskManagement.Api.Service.Interfaces
{
    public interface ITaskCommentService
    {
        Task<List<TaskCommentResponseDto>> GetByTaskAsync(Guid taskId, Guid currentUserId);
        Task<TaskCommentResponseDto> CreateAsync(Guid taskId, CreateTaskCommentRequestDto dto, Guid currentUserId);
        Task UpdateAsync(Guid commentId, string content, Guid currentUserId);
        Task DeleteAsync(Guid commentId, Guid currentUserId);
    }
}


