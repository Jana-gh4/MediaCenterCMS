namespace MediaCenterCMS.API.DTOs.News;

public class NewsResponse
{
    public int NewsId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime? ExpirationDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = string.Empty;
}