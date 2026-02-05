using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.Api.DTO.TaskDTO;
using TaskManagement.Api.Service.Interfaces;

namespace TaskManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _service;

        public TaskController(ITaskService service)
        {
            _service = service;
        }

        private Guid CurrentUserId =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskRequestDto dto)
        {
            await _service.CreateAsync(dto, CurrentUserId);
            return Ok();
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetByProject(Guid projectId)
        {
            return Ok(await _service.GetByProjectAsync(projectId, CurrentUserId));
        }

        [HttpPut("{taskId}")]
        public async Task<IActionResult> Update(Guid taskId, UpdateTaskRequestDto dto)
        {
            await _service.UpdateAsync(taskId, dto, CurrentUserId);
            return Ok();
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> Delete(Guid taskId)
        {
            await _service.DeleteAsync(taskId, CurrentUserId);
            return Ok();
        }
    }
}
