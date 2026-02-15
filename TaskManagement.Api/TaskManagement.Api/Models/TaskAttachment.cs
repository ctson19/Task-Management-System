using System;
using System.Collections.Generic;

namespace TaskManagement.Api.Models;

public partial class TaskAttachment
{
    public Guid Id { get; set; }

    public Guid TaskId { get; set; }

    public Guid OriginalFileId { get; set; }

    public int Version { get; set; }

    public string FileName { get; set; } = null!;

    public string StoredFileName { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public long FileSize { get; set; }

    public string? ContentType { get; set; }

    public Guid UploadedBy { get; set; }

    public DateTime UploadedAt { get; set; }

    public bool IsDeleted { get; set; }
}
