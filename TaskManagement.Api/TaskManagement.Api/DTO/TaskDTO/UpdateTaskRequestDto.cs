namespace TaskManagement.Api.DTO.TaskDTO
{
    public class UpdateTaskRequestDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int Status { get; set; }
        public int Priority { get; set; }
        public DateTime? Deadline { get; set; }
    }
}
