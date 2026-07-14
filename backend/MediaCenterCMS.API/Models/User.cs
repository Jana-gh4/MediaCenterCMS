namespace MediaCenterCMS.API.Models;

public class User
{
    public int UserId { get; set; }

    public int RoleId { get; set; }

    public string Username { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime? LastLoginAt { get; set; }

    // Navigation Property
    public Role Role { get; set; } = null!;

    // News
public ICollection<News> CreatedNews { get; set; } = new List<News>();

public ICollection<News> UpdatedNews { get; set; } = new List<News>();

// News Versions
public ICollection<NewsVersion> CreatedVersions { get; set; } = new List<NewsVersion>();

// Approval Requests
public ICollection<ApprovalRequest> RequestedApprovals { get; set; } = new List<ApprovalRequest>();

public ICollection<ApprovalRequest> ReviewedApprovals { get; set; } = new List<ApprovalRequest>();

// Audit Logs
public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
}