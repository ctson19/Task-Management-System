using TaskManagement.Api.DTO.TaskDTO;
using TaskManagement.Api.Models;
using TaskManagement.Api.Repository.Interfaces;
using TaskManagement.Api.Service.Interfaces;

namespace TaskManagement.Api.Service.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepo;
        private readonly IProjectMemberRepository _memberRepo;

        public TaskService(
            ITaskRepository taskRepo,
            IProjectMemberRepository memberRepo)
        {
            _taskRepo = taskRepo;
            _memberRepo = memberRepo;
        }

        public async Task CreateAsync(CreateTaskRequestDto dto, Guid userId)
        {
            if (!await _memberRepo.IsManagerAsync(dto.ProjectId, userId))
                throw new Exception("No permission");

            if (dto.AssignedTo.HasValue &&
                !await _memberRepo.ExistsAsync(dto.ProjectId, dto.AssignedTo.Value))
                throw new Exception("Assigned user is not project member");

            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                Status = 0,
                Deadline = dto.Deadline,
                ProjectId = dto.ProjectId,
                AssignedTo = dto.AssignedTo,
                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _taskRepo.AddAsync(task);
        }

        public async Task<List<TaskResponseDto>> GetByProjectAsync(Guid projectId, Guid userId)
        {
            if (!await _memberRepo.ExistsAsync(projectId, userId))
                throw new Exception("No permission");

            var tasks = await _taskRepo.GetByProjectAsync(projectId);

            return tasks.Select(MapToDto).ToList();
        }

        public async Task UpdateAsync(Guid taskId, UpdateTaskRequestDto dto, Guid userId)
        {
            var task = await _taskRepo.GetByIdAsync(taskId)
                ?? throw new Exception("Task not found");

            bool canEdit =
                await _memberRepo.IsManagerAsync(task.ProjectId, userId) ||
                task.AssignedTo == userId;

            if (!canEdit)
                throw new Exception("No permission");

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.Status = dto.Status;
            task.Priority = dto.Priority;
            task.Deadline = dto.Deadline;

            await _taskRepo.UpdateAsync(task);
        }

        public async Task DeleteAsync(Guid taskId, Guid userId)
        {
            var task = await _taskRepo.GetByIdAsync(taskId)
                ?? throw new Exception("Task not found");

            if (!await _memberRepo.IsManagerAsync(task.ProjectId, userId))
                throw new Exception("No permission");

            await _taskRepo.DeleteAsync(task);
        }

        private static TaskResponseDto MapToDto(TaskItem task)
        {
            return new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                Priority = task.Priority,
                Deadline = task.Deadline,
                AssignedTo = task.AssignedTo
            };
        }
    }
}