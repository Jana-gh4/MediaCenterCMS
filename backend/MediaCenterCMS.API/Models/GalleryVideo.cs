using System.ComponentModel.DataAnnotations;
using MediaCenterCMS.API.Enums;

namespace MediaCenterCMS.API.Models;

public class GalleryVideo
{
    public int GalleryVideoId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public int MediaId { get; set; }

    public int? ApprovalRequestId { get; set; }

    public VisibilityStatus VisibilityStatus { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    // Navigation Properties
    public Media Media { get; set; } = null!;

    public ApprovalRequest? ApprovalRequest { get; set; }

    public User Creator { get; set; } = null!;
}