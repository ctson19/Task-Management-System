using System;
using System.Collections.Generic;

namespace TaskManagement.Api.Models;

public partial class EmailOtp
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string OtpCode { get; set; } = null!;

    public DateTime ExpiredAt { get; set; }

    public bool IsUsed { get; set; }

    public DateTime CreatedAt { get; set; }
}
