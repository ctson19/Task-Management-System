namespace TaskManagement.Api.DTO.TaskDTO
{
    public class TaskResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int Status { get; set; }
        public int Priority { get; set; }
        public DateTime? Deadline { get; set; }
        public Guid? AssignedTo { get; set; }
    }
}
