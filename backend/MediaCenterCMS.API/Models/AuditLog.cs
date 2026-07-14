using System.ComponentModel.DataAnnotations;

namespace MediaCenterCMS.API.Models;

public class AuditLog
{
    public int AuditLogId { get; set; }

    public int UserId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Action { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string EntityName { get; set; } = string.Empty;

    public int EntityId { get; set; }

    public DateTime Timestamp { get; set; }

    // Navigation Property
    public User User { get; set; } = null!;
}