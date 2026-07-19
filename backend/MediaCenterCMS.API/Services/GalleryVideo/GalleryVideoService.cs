using MediaCenterCMS.API.Data;
using MediaCenterCMS.API.DTOs.GalleryVideo;
using MediaCenterCMS.API.Enums;
using MediaCenterCMS.API.Models;
using MediaCenterCMS.API.Services.Audit;
using MediaCenterCMS.API.Services.Media;
using Microsoft.EntityFrameworkCore;

namespace MediaCenterCMS.API.Services.GalleryVideo;

public class GalleryVideoService : IGalleryVideoService
{
    private readonly AppDbContext _context;
    private readonly IMediaService _mediaService;
    private readonly IAuditService _auditService;

    public GalleryVideoService(
        AppDbContext context,
        IMediaService mediaService,
        IAuditService auditService)
    {
        _context = context;
        _mediaService = mediaService;
        _auditService = auditService;
    }

    public async Task<GalleryVideoResponse> CreateAsync(
        CreateGalleryVideoRequest request,
        int userId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // Upload the video
            var uploadResult = await _mediaService.UploadAsync(
                request.Video,
                userId);

            // Create gallery video
            var galleryVideo = new Models.GalleryVideo
            {
                Title = request.Title,
                MediaId = uploadResult.MediaId,

                VisibilityStatus = VisibilityStatus.Hidden,

                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.GalleryVideos.Add(galleryVideo);

            await _context.SaveChangesAsync();

            // Create approval request
            var approvalRequest = new ApprovalRequest
            {
                EntityType = ApprovalEntityType.GalleryVideo,
                EntityId = galleryVideo.GalleryVideoId,

                RequestedBy = userId,
                RequestedAt = DateTime.UtcNow,

                Status = ApprovalStatus.Pending
            };

            _context.ApprovalRequests.Add(approvalRequest);

            await _context.SaveChangesAsync();

            // Link the approval request
            galleryVideo.ApprovalRequestId = approvalRequest.ApprovalRequestId;

            await _context.SaveChangesAsync();

            // Audit log
            await _auditService.LogAsync(
                userId,
                "Create Gallery Video",
                "GalleryVideo",
                galleryVideo.GalleryVideoId);

            await transaction.CommitAsync();

            var creatorUsername = await _context.Users
                .Where(u => u.UserId == userId)
                .Select(u => u.Username)
                .FirstAsync();

            return new GalleryVideoResponse
            {
                GalleryVideoId = galleryVideo.GalleryVideoId,
                Title = galleryVideo.Title,
                VideoPath = uploadResult.FilePath,
                CreatedBy = creatorUsername,
                CreatedAt = galleryVideo.CreatedAt,
                Status = approvalRequest.Status.ToString()
            };
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    public async Task<IEnumerable<GalleryVideoResponse>> GetAllAsync()
    {
        return await _context.GalleryVideos
            .Include(g => g.Media)
            .Include(g => g.Creator)
            .Include(g => g.ApprovalRequest)
            .Select(g => new GalleryVideoResponse
            {
                GalleryVideoId = g.GalleryVideoId,
                Title = g.Title,
                VideoPath = g.Media.FilePath,
                CreatedBy = g.Creator.Username,
                CreatedAt = g.CreatedAt,
                Status = g.ApprovalRequest != null
                    ? g.ApprovalRequest.Status.ToString()
                    : "Pending"
            })
            .ToListAsync();
    }

    public async Task<GalleryVideoResponse?> GetByIdAsync(int id)
    {
        return await _context.GalleryVideos
            .Include(g => g.Media)
            .Include(g => g.Creator)
            .Include(g => g.ApprovalRequest)
            .Where(g => g.GalleryVideoId == id)
            .Select(g => new GalleryVideoResponse
            {
                GalleryVideoId = g.GalleryVideoId,
                Title = g.Title,
                VideoPath = g.Media.FilePath,
                CreatedBy = g.Creator.Username,
                CreatedAt = g.CreatedAt,
                Status = g.ApprovalRequest != null
                    ? g.ApprovalRequest.Status.ToString()
                    : "Pending"
            })
            .FirstOrDefaultAsync();
    }
    public async Task<bool> SetVisibilityAsync(
        int id,
        VisibilityStatus visibilityStatus)
    {
        var galleryVideo = await _context.GalleryVideos
            .FirstOrDefaultAsync(g => g.GalleryVideoId == id);

        if (galleryVideo == null)
            return false;

        galleryVideo.VisibilityStatus = visibilityStatus;

        await _context.SaveChangesAsync();

        return true;
    }
}