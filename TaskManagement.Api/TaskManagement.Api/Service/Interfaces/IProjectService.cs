using TaskManagement.Api.DTO.Project;

namespace TaskManagement.Api.Service.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectResponseDto> CreateAsync(CreateProjectRequestDto request, Guid userId);
        Task<List<ProjectResponseDto>> GetMyProjectsAsync(Guid userId);
        Task<ProjectResponseDto> UpdateAsync(Guid projectId, UpdateProjectRequestDto request, Guid userId);
        Task DeleteAsync(Guid projectId, Guid userId);
    }
}
