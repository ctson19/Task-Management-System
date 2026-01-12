using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.Api.DTO.Project;
using TaskManagement.Api.Service.Interfaces;

namespace TaskManagement.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        private Guid GetUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User not authenticated");

            return Guid.Parse(userId);
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectRequestDto request)
        {
            var result = await _projectService.CreateAsync(request, GetUserId());
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetMyProjects()
        {
            var result = await _projectService.GetMyProjectsAsync(GetUserId());
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateProjectRequestDto request)
        {
            var result = await _projectService.UpdateAsync(id, request, GetUserId());
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _projectService.DeleteAsync(id, GetUserId());
            return NoContent();
        }
    }
}
