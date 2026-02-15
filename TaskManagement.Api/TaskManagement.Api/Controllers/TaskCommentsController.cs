using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.Api.DTO.TaskComments;
using TaskManagement.Api.Service.Interfaces;

namespace TaskManagement.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/tasks/{taskId}/comments")]
    public class TaskCommentsController : ControllerBase
    {
        private readonly ITaskCommentService _service;

        public TaskCommentsController(ITaskCommentService service)
        {
            _service = service;
        }

        private Guid CurrentUserId =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<IActionResult> Get(Guid taskId)
        {
            var result = await _service.GetByTaskAsync(taskId, CurrentUserId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Guid taskId, [FromBody] CreateTaskCommentRequestDto dto)
        {
            var result = await _service.CreateAsync(taskId, dto, CurrentUserId);
            return Ok(result);
        }

        [HttpPut("{commentId}")]
        public async Task<IActionResult> Update(Guid taskId, Guid commentId, [FromBody] CreateTaskCommentRequestDto dto)
        {
            await _service.UpdateAsync(commentId, dto.Content, CurrentUserId);
            return Ok();
        }

        [HttpDelete("{commentId}")]
        public async Task<IActionResult> Delete(Guid taskId, Guid commentId)
        {
            await _service.DeleteAsync(commentId, CurrentUserId);
            return Ok();
        }
    }
}


