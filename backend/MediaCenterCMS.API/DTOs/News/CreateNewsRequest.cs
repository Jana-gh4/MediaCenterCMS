using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MediaCenterCMS.API.DTOs.News;

public class CreateNewsRequest
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    public DateTime? ExpirationDate { get; set; }

    public IFormFile? CoverImage { get; set; }
}