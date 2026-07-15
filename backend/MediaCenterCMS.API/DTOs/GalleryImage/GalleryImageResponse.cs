namespace MediaCenterCMS.API.DTOs.GalleryImage;

public class GalleryImageResponse
{
    public int GalleryImageId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string ImagePath { get; set; } = string.Empty;

    public string CreatedBy { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public string Status { get; set; } = string.Empty;
}