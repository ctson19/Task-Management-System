using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.Api.DTO.ProjectMembers;
using TaskManagement.Api.Service.Interfaces;

namespace TaskManagement.Api.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId}/members")]
    [Authorize]
    public class ProjectMembersController : ControllerBase
    {
        private readonly IProjectMemberService _service;

        public ProjectMembersController(IProjectMemberService service)
        {
            _service = service;
        }

        private Guid CurrentUserId =>
            Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        // =========================
        // GET: /api/projects/{projectId}/members
        // =========================
        [HttpGet]
        public async Task<IActionResult> GetMembers(Guid projectId)
        {
            var result = await _service.GetMembersAsync(projectId);
            return Ok(result);
        }

        // =========================
        // POST: /api/projects/{projectId}/members
        // =========================
        [HttpPost]
        public async Task<IActionResult> AddMember(
            Guid projectId,
            [FromBody] AddProjectMemberDto dto)
        {
            dto.ProjectId = projectId;

            await _service.AddMemberAsync(dto, CurrentUserId);
            return Ok(new { message = "Member added successfully" });
        }

        // =========================
        // PUT: /api/projects/{projectId}/members/{userId}/role
        // =========================
        [HttpPut("{userId}/role")]
        public async Task<IActionResult> UpdateRole(
            Guid projectId,
            Guid userId,
            [FromQuery] string role)
        {
            await _service.UpdateRoleAsync(projectId, userId, role, CurrentUserId);
            return Ok(new { message = "Role updated successfully" });
        }

        // =========================
        // DELETE: /api/projects/{projectId}/members/{userId}
        // =========================
        [HttpDelete("{userId}")]
        public async Task<IActionResult> RemoveMember(
            Guid projectId,
            Guid userId)
        {
            await _service.RemoveMemberAsync(projectId, userId, CurrentUserId);
            return Ok(new { message = "Member removed successfully" });
        }
    }
}