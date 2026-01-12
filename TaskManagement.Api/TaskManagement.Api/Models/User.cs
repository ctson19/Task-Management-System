using System;
using System.Collections.Generic;

namespace TaskManagement.Api.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual ICollection<TaskItem> TaskAssignedToNavigations { get; set; } = new List<TaskItem>();

    public virtual ICollection<TaskComment> TaskComments { get; set; } = new List<TaskComment>();

    public virtual ICollection<TaskItem> TaskCreatedByNavigations { get; set; } = new List<TaskItem>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
    public virtual ICollection<ProjectMember> ProjectMembers { get; set; }
}
