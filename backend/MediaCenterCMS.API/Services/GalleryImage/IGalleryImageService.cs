using MediaCenterCMS.API.DTOs.GalleryImage;
using MediaCenterCMS.API.Enums;

namespace MediaCenterCMS.API.Services.GalleryImage;

public interface IGalleryImageService
{
    Task<GalleryImageResponse> CreateAsync(
        CreateGalleryImageRequest request,
        int userId);

    Task<IEnumerable<GalleryImageResponse>> GetAllAsync();

    Task<GalleryImageResponse?> GetByIdAsync(int id);

    Task<bool> SetVisibilityAsync(
        int id,
        VisibilityStatus visibilityStatus);
}