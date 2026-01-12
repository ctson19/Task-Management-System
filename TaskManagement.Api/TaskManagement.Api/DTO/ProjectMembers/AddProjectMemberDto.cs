namespace TaskManagement.Api.DTO.ProjectMembers
{
    public class AddProjectMemberDto
    {
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
        public string Role { get; set; } = "Member";
    }
}

