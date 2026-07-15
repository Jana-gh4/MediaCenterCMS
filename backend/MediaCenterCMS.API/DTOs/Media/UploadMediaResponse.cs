namespace MediaCenterCMS.API.DTOs.Media;

public class UploadMediaResponse
{
    public int MediaId { get; set; }

    public string FilePath { get; set; } = string.Empty;
}