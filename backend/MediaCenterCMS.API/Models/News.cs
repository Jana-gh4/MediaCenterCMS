using System.ComponentModel.DataAnnotations;

namespace MediaCenterCMS.API.Models;

public class News
{
    public int NewsId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    public DateTime? ExpirationDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? CurrentVersionId { get; set; }

    // Navigation Properties
    public User Creator { get; set; } = null!;

    public User? Updater { get; set; }

    public NewsVersion? CurrentVersion { get; set; }

    public ICollection<NewsVersion> Versions { get; set; } = new List<NewsVersion>();

}