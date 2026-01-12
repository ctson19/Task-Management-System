namespace TaskManagement.Api.Models
{
    public partial class ProjectMember
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
        public string Role { get; set; }
        public DateTime JoinedAt { get; set; }

        public virtual Project Project { get; set; }
        public virtual User User { get; set; }
    }

}
