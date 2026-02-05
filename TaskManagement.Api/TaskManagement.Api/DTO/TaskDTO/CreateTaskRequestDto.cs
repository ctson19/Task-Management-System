namespace TaskManagement.Api.DTO.TaskDTO
{
    public class CreateTaskRequestDto
    {
        public Guid ProjectId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int Priority { get; set; }
        public DateTime? Deadline { get; set; }
        public Guid? AssignedTo { get; set; }
    }
}
