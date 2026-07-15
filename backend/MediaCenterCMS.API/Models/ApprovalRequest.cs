using System.ComponentModel.DataAnnotations;
using MediaCenterCMS.API.Enums;

namespace MediaCenterCMS.API.Models;

public class ApprovalRequest
{
    public int ApprovalRequestId { get; set; }

    public ApprovalEntityType EntityType { get; set; }

    public int EntityId { get; set; }

    public int RequestedBy { get; set; }

    public DateTime RequestedAt { get; set; }

    public int? ReviewedBy { get; set; }

    public DateTime? ReviewedAt { get; set; }

    public ApprovalStatus Status { get; set; }

    [MaxLength(500)]
    public string? RejectionReason { get; set; }

    // Navigation Properties
    public User Requester { get; set; } = null!;

    public User? Reviewer { get; set; }
}