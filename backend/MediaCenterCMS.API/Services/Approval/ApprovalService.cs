using MediaCenterCMS.API.Data;
using MediaCenterCMS.API.DTOs.Approval;
using MediaCenterCMS.API.DTOs.News;
using Microsoft.EntityFrameworkCore;
using MediaCenterCMS.API.Enums;
using MediaCenterCMS.API.Services.Audit;

namespace MediaCenterCMS.API.Services.Approval;

public class ApprovalService : IApprovalService
{
    private readonly AppDbContext _context;
    private readonly IAuditService _auditService;

    public ApprovalService(
        AppDbContext context,
        IAuditService auditService)
    {
        _context = context;
        _auditService = auditService;
    }

    public async Task<IEnumerable<ApprovalResponse>> GetPendingAsync()
    {
        var requests = await _context.ApprovalRequests
            .Include(a => a.Requester)
            .Where(a => a.Status == ApprovalStatus.Pending)
            .OrderByDescending(a => a.RequestedAt)
            .ToListAsync();

        var result = new List<ApprovalResponse>();

        foreach (var request in requests)
        {
            var title = string.Empty;

            switch (request.EntityType)
            {
                case ApprovalEntityType.News:

                    var version = await _context.NewsVersions
                        .FirstOrDefaultAsync(v =>
                            v.ApprovalRequestId == request.ApprovalRequestId);

                    title = version?.Title ?? "News";

                    break;

                case ApprovalEntityType.GalleryImage:

                    var image = await _context.GalleryImages
                        .FirstOrDefaultAsync(g =>
                            g.GalleryImageId == request.EntityId);

                    title = image?.Title ?? "Image";

                    break;

                case ApprovalEntityType.GalleryVideo:

                    var video = await _context.GalleryVideos
                        .FirstOrDefaultAsync(v =>
                            v.GalleryVideoId == request.EntityId);

                    title = video?.Title ?? "Video";

                    break;
            }

            result.Add(new ApprovalResponse
            {
                ApprovalRequestId = request.ApprovalRequestId,
                EntityType = request.EntityType.ToString(),
                EntityId = request.EntityId,
                Title = title,
                RequestedBy = request.Requester.Username,
                RequestedAt = request.RequestedAt,
                Status = request.Status.ToString()
            });
        }

        return result;
    }

    public async Task<NewsResponse?> ApproveAsync(
        int approvalRequestId,
        int reviewerId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var approvalRequest = await _context.ApprovalRequests
                .FirstOrDefaultAsync(a => a.ApprovalRequestId == approvalRequestId);

            if (approvalRequest == null)
                return null;

            if (approvalRequest.EntityType == ApprovalEntityType.GalleryImage)
            {
                var galleryImage = await _context.GalleryImages
                    .FirstOrDefaultAsync(g =>
                        g.GalleryImageId == approvalRequest.EntityId);

                if (galleryImage == null)
                    return null;

                approvalRequest.Status = ApprovalStatus.Approved;
                approvalRequest.ReviewedBy = reviewerId;
                approvalRequest.ReviewedAt = DateTime.UtcNow;

                galleryImage.VisibilityStatus = VisibilityStatus.Visible;

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                await _auditService.LogAsync(
                    reviewerId,
                    "Approve Gallery Image",
                    "GalleryImage",
                    galleryImage.GalleryImageId);

                return new NewsResponse();
            }

            if (approvalRequest.EntityType == ApprovalEntityType.GalleryVideo)
            {
                var galleryVideo = await _context.GalleryVideos
                    .FirstOrDefaultAsync(v =>
                        v.GalleryVideoId == approvalRequest.EntityId);

                if (galleryVideo == null)
                    return null;

                Console.WriteLine($"Found GalleryVideo {galleryVideo.GalleryVideoId}");

                approvalRequest.Status = ApprovalStatus.Approved;
                approvalRequest.ReviewedBy = reviewerId;
                approvalRequest.ReviewedAt = DateTime.UtcNow;

                galleryVideo.VisibilityStatus = VisibilityStatus.Visible;

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                await _auditService.LogAsync(
                    reviewerId,
                    "Approve Gallery Video",
                    "GalleryVideo",
                    galleryVideo.GalleryVideoId);

                return new NewsResponse();
            }

            var version = await _context.NewsVersions
                .FirstOrDefaultAsync(v =>
                    v.ApprovalRequestId == approvalRequestId);

            if (version == null)
                return null;

            var news = await _context.News
                .Include(n => n.Creator)
                .FirstOrDefaultAsync(n => n.NewsId == version.NewsId);

            if (news == null)
                return null;

            approvalRequest.Status = ApprovalStatus.Approved;
            approvalRequest.ReviewedBy = reviewerId;
            approvalRequest.ReviewedAt = DateTime.UtcNow;

            version.ApprovalStatus = ApprovalStatus.Approved;

            news.CurrentVersionId = version.NewsVersionId;

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            await _auditService.LogAsync(
                reviewerId,
                "Approve News",
                "NewsVersion",
                version.NewsVersionId);

            return new NewsResponse
            {
                NewsId = news.NewsId,
                Title = version.Title,
                Content = version.Content,
                ExpirationDate = news.ExpirationDate,
                CreatedAt = news.CreatedAt,
                CreatedBy = news.Creator.Username
            };
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> RejectAsync(
        int approvalRequestId,
        int reviewerId,
        string reason)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var approvalRequest = await _context.ApprovalRequests
                .FirstOrDefaultAsync(a => a.ApprovalRequestId == approvalRequestId);

            if (approvalRequest == null)
                return false;

            if (approvalRequest.EntityType != ApprovalEntityType.News)
            {
                throw new NotImplementedException();
            }

            var version = await _context.NewsVersions
                .FirstOrDefaultAsync(v => v.ApprovalRequestId == approvalRequestId);

            if (version == null)
                return false;

            approvalRequest.Status = ApprovalStatus.Rejected;
            approvalRequest.ReviewedBy = reviewerId;
            approvalRequest.ReviewedAt = DateTime.UtcNow;
            approvalRequest.RejectionReason = reason;

            version.ApprovalStatus = ApprovalStatus.Rejected;

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            await _auditService.LogAsync(
                reviewerId,
                "Reject News",
                "NewsVersion",
                version.NewsVersionId);

            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}