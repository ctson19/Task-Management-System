namespace TaskManagement.Api.DTO.ProjectMembers
{
    public class ProjectMemberDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
