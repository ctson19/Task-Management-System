using TaskManagement.Api.DTO.TaskDTO;
using TaskManagement.Api.Models;

namespace TaskManagement.Api.Service.Interfaces
{
    public interface ITaskService
    {
        Task CreateAsync(CreateTaskRequestDto dto, Guid userId);
        Task<List<TaskResponseDto>> GetByProjectAsync(Guid projectId, Guid userId);
        Task UpdateAsync(Guid taskId, UpdateTaskRequestDto dto, Guid userId);
        Task DeleteAsync(Guid taskId, Guid userId);
        TaskResponseDto MapToDto(TaskItem task);
    }
}
