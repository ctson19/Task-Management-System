using TaskManagement.Api.DTO.Project;
using TaskManagement.Api.Models;
using TaskManagement.Api.Repository.Interfaces;
using TaskManagement.Api.Service.Interfaces;

namespace TaskManagement.Api.Service.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;

        public ProjectService(
            IProjectRepository projectRepository,
            IProjectMemberRepository projectMemberRepository)
        {
            _projectRepository = projectRepository;
            _projectMemberRepository = projectMemberRepository;
        }

        public async Task<ProjectResponseDto> CreateAsync(
    CreateProjectRequestDto request,
    Guid userId)
        {
            var project = new Project
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow
            };

            // 1️⃣ Tạo project
            await _projectRepository.AddAsync(project);

            // 2️⃣ Gắn OWNER vào ProjectMembers
            await _projectMemberRepository.AddAsync(new ProjectMember
            {
                ProjectId = project.Id,
                UserId = userId,
                Role = "Owner",
                JoinedAt = DateTime.UtcNow
            });

            // 3️⃣ Save chung
            await _projectMemberRepository.SaveChangesAsync();

            return MapToDto(project);
        }

        public async Task<List<ProjectResponseDto>> GetMyProjectsAsync(Guid userId)
        {
            var projects = await _projectRepository.GetByUserAsync(userId);
            return projects.Select(MapToDto).ToList();
        }

        public async Task<ProjectResponseDto> UpdateAsync(Guid projectId, UpdateProjectRequestDto request, Guid userId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId)
                ?? throw new Exception("Project not found");

            if (project.CreatedBy != userId)
                throw new Exception("Không có quyền sửa project này");

            project.Name = request.Name;
            project.Description = request.Description;

            await _projectRepository.UpdateAsync(project);

            return MapToDto(project);
        }

        public async Task DeleteAsync(Guid projectId, Guid userId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId)
                ?? throw new Exception("Project not found");

            if (project.CreatedBy != userId)
                throw new Exception("Không có quyền xóa project này");

            await _projectRepository.DeleteAsync(project);
        }

        private static ProjectResponseDto MapToDto(Project project)
        {
            return new ProjectResponseDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CreatedBy = project.CreatedBy,
                CreatedAt = project.CreatedAt
            };
        }
    }
}
