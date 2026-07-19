using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MediaCenterCMS.API.DTOs.GalleryVideo;

public class CreateGalleryVideoRequest
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public IFormFile Video { get; set; } = null!;
}