using MediaCenterCMS.API.DTOs.Media;
using Microsoft.AspNetCore.Http;

namespace MediaCenterCMS.API.Services.Media;

public interface IMediaService
{
    Task<UploadMediaResponse> UploadAsync(
        IFormFile file,
        int userId);
}