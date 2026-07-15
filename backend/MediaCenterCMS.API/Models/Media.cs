using System.ComponentModel.DataAnnotations;
using MediaCenterCMS.API.Enums;

namespace MediaCenterCMS.API.Models;

public class Media
{
    public int MediaId { get; set; }

    [Required]
    [MaxLength(255)]
    public string FileName { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string FilePath { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string ContentType { get; set; } = string.Empty;

    public MediaType MediaType { get; set; }

    public int UploadedBy { get; set; }

    public DateTime UploadedAt { get; set; }

    // Navigation
    public User Uploader { get; set; } = null!;
}