using MediaCenterCMS.API.DTOs.GalleryVideo;
using MediaCenterCMS.API.Enums;

namespace MediaCenterCMS.API.Services.GalleryVideo;

public interface IGalleryVideoService
{
    Task<GalleryVideoResponse> CreateAsync(
        CreateGalleryVideoRequest request,
        int userId);

    Task<IEnumerable<GalleryVideoResponse>> GetAllAsync();

    Task<GalleryVideoResponse?> GetByIdAsync(int id);

    Task<bool> SetVisibilityAsync(
        int id,
        VisibilityStatus visibilityStatus);
}