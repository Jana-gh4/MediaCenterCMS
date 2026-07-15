using MediaCenterCMS.API.Data;
using MediaCenterCMS.API.DTOs.Media;
using MediaCenterCMS.API.Enums;
using MediaCenterCMS.API.Models;
using Microsoft.AspNetCore.Http;
using MediaEntity = MediaCenterCMS.API.Models.Media;

namespace MediaCenterCMS.API.Services.Media;

public class MediaService : IMediaService
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public MediaService(
        AppDbContext context,
        IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task<UploadMediaResponse> UploadAsync(
        IFormFile file,
        int userId)
    {
        if (file == null || file.Length == 0)
        {
            throw new Exception("No file was uploaded.");
        }

        var extension = Path.GetExtension(file.FileName).ToLower();

        MediaType mediaType;

        if (extension == ".jpg" ||
            extension == ".jpeg" ||
            extension == ".png")
        {
            mediaType = MediaType.Image;
        }
        else if (extension == ".mp4" ||
                extension == ".mov")
        {
            mediaType = MediaType.Video;
        }
        else
        {
            throw new Exception("Unsupported file type.");
        }

        var folder = mediaType == MediaType.Image
            ? "images"
            : "videos";

        var uniqueFileName =
            $"{Guid.NewGuid()}{extension}";

        var relativePath =
            Path.Combine("uploads", folder, uniqueFileName);

        var absolutePath =
            Path.Combine(_environment.WebRootPath, relativePath);

        Directory.CreateDirectory(Path.GetDirectoryName(absolutePath)!);

        await using (var stream = new FileStream(absolutePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var media = new MediaEntity
        {
            FileName = file.FileName,
            FilePath = relativePath.Replace("\\", "/"),
            ContentType = file.ContentType,
            MediaType = mediaType,
            UploadedBy = userId,
            UploadedAt = DateTime.UtcNow
        };

        _context.Media.Add(media);

        await _context.SaveChangesAsync();

        return new UploadMediaResponse
        {
            MediaId = media.MediaId,
            FilePath = media.FilePath
        };
    }
}