namespace MediaCenterCMS.API.DTOs.GalleryVideo;

public class GalleryVideoResponse
{
    public int GalleryVideoId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string VideoPath { get; set; } = string.Empty;

    public string CreatedBy { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public string Status { get; set; } = string.Empty;
}