using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MediaCenterCMS.API.DTOs.GalleryImage;

public class CreateGalleryImageRequest
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public IFormFile Image { get; set; } = null!;
}