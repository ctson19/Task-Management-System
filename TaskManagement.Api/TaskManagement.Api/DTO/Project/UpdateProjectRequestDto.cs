using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Api.DTO.Project
{
    public class UpdateProjectRequestDto
    {
        [Required]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}
