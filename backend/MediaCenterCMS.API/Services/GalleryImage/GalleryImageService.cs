using MediaCenterCMS.API.Data;
using MediaCenterCMS.API.DTOs.GalleryImage;
using MediaCenterCMS.API.Enums;
using MediaCenterCMS.API.Models;
using MediaCenterCMS.API.Services.Audit;
using MediaCenterCMS.API.Services.Media;
using Microsoft.EntityFrameworkCore;

namespace MediaCenterCMS.API.Services.GalleryImage;

public class GalleryImageService : IGalleryImageService
{
    private readonly AppDbContext _context;
    private readonly IMediaService _mediaService;
    private readonly IAuditService _auditService;

    public GalleryImageService(
        AppDbContext context,
        IMediaService mediaService,
        IAuditService auditService)
    {
        _context = context;
        _mediaService = mediaService;
        _auditService = auditService;
    }

    public async Task<GalleryImageResponse> CreateAsync(
        CreateGalleryImageRequest request,
        int userId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // Upload the image
            var uploadResult = await _mediaService.UploadAsync(
                request.Image,
                userId);

            // Create gallery image
            var galleryImage = new Models.GalleryImage
            {
                Title = request.Title,
                MediaId = uploadResult.MediaId,

                VisibilityStatus = VisibilityStatus.Hidden,

                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.GalleryImages.Add(galleryImage);

            await _context.SaveChangesAsync();

            // Create approval request
            var approvalRequest = new ApprovalRequest
            {
                EntityType = ApprovalEntityType.GalleryImage,
                EntityId = galleryImage.GalleryImageId,

                RequestedBy = userId,
                RequestedAt = DateTime.UtcNow,

                Status = ApprovalStatus.Pending
            };

            _context.ApprovalRequests.Add(approvalRequest);

            await _context.SaveChangesAsync();

            // Link the approval request
            galleryImage.ApprovalRequestId = approvalRequest.ApprovalRequestId;

            await _context.SaveChangesAsync();

            // Audit log
            await _auditService.LogAsync(
                userId,
                "Create Gallery Image",
                "GalleryImage",
                galleryImage.GalleryImageId);

            await transaction.CommitAsync();

            var creatorUsername = await _context.Users
                .Where(u => u.UserId == userId)
                .Select(u => u.Username)
                .FirstAsync();

            return new GalleryImageResponse
            {
                GalleryImageId = galleryImage.GalleryImageId,
                Title = galleryImage.Title,
                ImagePath = uploadResult.FilePath,
                CreatedBy = creatorUsername,
                CreatedAt = galleryImage.CreatedAt,
                Status = approvalRequest.Status.ToString()
            };
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    public async Task<IEnumerable<GalleryImageResponse>> GetAllAsync()
    {
        return await _context.GalleryImages
            .Include(g => g.Media)
            .Include(g => g.Creator)
            .Include(g => g.ApprovalRequest)
            .Select(g => new GalleryImageResponse
            {
                GalleryImageId = g.GalleryImageId,
                Title = g.Title,
                ImagePath = g.Media.FilePath,
                CreatedBy = g.Creator.Username,
                CreatedAt = g.CreatedAt,
                Status = g.ApprovalRequest != null
                    ? g.ApprovalRequest.Status.ToString()
                    : "Pending"
            })
            .ToListAsync();
    }

    public async Task<GalleryImageResponse?> GetByIdAsync(int id)
    {
        return await _context.GalleryImages
            .Include(g => g.Media)
            .Include(g => g.Creator)
            .Include(g => g.ApprovalRequest)
            .Where(g => g.GalleryImageId == id)
            .Select(g => new GalleryImageResponse
            {
                GalleryImageId = g.GalleryImageId,
                Title = g.Title,
                ImagePath = g.Media.FilePath,
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
        var galleryImage = await _context.GalleryImages
            .FirstOrDefaultAsync(g => g.GalleryImageId == id);

        if (galleryImage == null)
            return false;

        galleryImage.VisibilityStatus = visibilityStatus;

        await _context.SaveChangesAsync();

        return true;
    }
}