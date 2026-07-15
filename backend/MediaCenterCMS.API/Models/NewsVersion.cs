using System.ComponentModel.DataAnnotations;
using MediaCenterCMS.API.Enums;

namespace MediaCenterCMS.API.Models;

public class NewsVersion
{
    public int NewsVersionId { get; set; }
    public int NewsId { get; set; }

    public int VersionNumber { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public ApprovalStatus ApprovalStatus { get; set; }

    public int? ApprovalRequestId { get; set; }

    // Navigation Properties
    public News News { get; set; } = null!;

    public User Creator { get; set; } = null!;

    public ApprovalRequest? ApprovalRequest { get; set; }

    public int? CoverMediaId { get; set; }
    
    public Media? CoverMedia { get; set; }
}