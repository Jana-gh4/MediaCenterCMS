using MediaCenterCMS.API.Data;
using MediaCenterCMS.API.DTOs.News;
using MediaCenterCMS.API.Enums;
using MediaCenterCMS.API.Models;
using Microsoft.EntityFrameworkCore;
using NewsEntity = MediaCenterCMS.API.Models.News;
using MediaCenterCMS.API.Services.Audit;
using MediaCenterCMS.API.Services.Media;

namespace MediaCenterCMS.API.Services.News;

public class NewsService : INewsService
{
    private readonly AppDbContext _context;
    private readonly IAuditService _auditService;
    private readonly IMediaService _mediaService;

    public NewsService(
        AppDbContext context,
        IAuditService auditService,
        IMediaService mediaService)
    {
        _context = context;
        _auditService = auditService;
        _mediaService = mediaService;
    }

    public async Task<NewsResponse> CreateAsync(
    CreateNewsRequest request,
    int userId)
{
    await using var transaction = await _context.Database.BeginTransactionAsync();

    try
    {
        var news = new NewsEntity
        {
            Title = request.Title,
            Content = request.Content,
            ExpirationDate = request.ExpirationDate,
            CreatedBy = userId,
            CreatedAt = DateTime.UtcNow
        };

        _context.News.Add(news);
        await _context.SaveChangesAsync();

        int? coverMediaId = null;

        if (request.CoverImage != null)
        {
            var uploadResult = await _mediaService.UploadAsync(
                request.CoverImage,
                userId);

            coverMediaId = uploadResult.MediaId;
        }

        var version = new NewsVersion
        {
            NewsId = news.NewsId,
            VersionNumber = 1,
            Title = news.Title,
            Content = news.Content,
            CoverMediaId = coverMediaId,
            CreatedBy = userId,
            CreatedAt = DateTime.UtcNow,
            ApprovalStatus = ApprovalStatus.Pending
        };

        _context.NewsVersions.Add(version);
        await _context.SaveChangesAsync();

        await transaction.CommitAsync();

        await _auditService.LogAsync(
            userId,
            "Create News",
            "News",
            news.NewsId);

       var creatorUsername = await _context.Users
        .Where(u => u.UserId == userId)
        .Select(u => u.Username)
        .FirstAsync();

        return new NewsResponse
        {
            NewsId = news.NewsId,
            Title = news.Title,
            Content = news.Content,
            ExpirationDate = news.ExpirationDate,
            CreatedAt = news.CreatedAt,
            CreatedBy = creatorUsername
        };
    }
    catch
    {
        await transaction.RollbackAsync();
        throw;
    }
}
    public async Task<IEnumerable<NewsResponse>> GetAllAsync()
    {
        return await _context.News
            .Include(n => n.Creator)
            .Include(n => n.CurrentVersion)
                .ThenInclude(v => v.CoverMedia)
            .Select(n => new NewsResponse
            {
                NewsId = n.NewsId,
                Title = n.Title,
                Content = n.Content,
                ExpirationDate = n.ExpirationDate,
                CreatedAt = n.CreatedAt,
                CreatedBy = n.Creator.Username,
                CoverImagePath = n.CurrentVersion != null &&
                                n.CurrentVersion.CoverMedia != null
                    ? n.CurrentVersion.CoverMedia.FilePath
                    : null
            })
            .ToListAsync();
    }
    public async Task<NewsResponse?> GetByIdAsync(int id)
    {
        return await _context.News
            .Include(n => n.Creator)
            .Include(n => n.CurrentVersion)
                .ThenInclude(v => v.CoverMedia)
            .Where(n => n.NewsId == id)
            .Select(n => new NewsResponse
            {
                NewsId = n.NewsId,
                Title = n.Title,
                Content = n.Content,
                ExpirationDate = n.ExpirationDate,
                CreatedAt = n.CreatedAt,
                CreatedBy = n.Creator.Username,
                CoverImagePath = n.CurrentVersion != null &&
                                n.CurrentVersion.CoverMedia != null
                    ? n.CurrentVersion.CoverMedia.FilePath
                    : null
            })
            .FirstOrDefaultAsync();
    }
    public async Task<NewsResponse?> UpdateAsync(
        int id,
        UpdateNewsRequest request,
        int userId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // Find the news
            var news = await _context.News
                .FirstOrDefaultAsync(n => n.NewsId == id);

            if (news == null)
                return null;

            // Update the working copy
            news.Title = request.Title;
            news.Content = request.Content;
            news.ExpirationDate = request.ExpirationDate;

            news.UpdatedBy = userId;
            news.UpdatedAt = DateTime.UtcNow;

            // Find the latest version
            var latestVersion = await _context.NewsVersions
                .Where(v => v.NewsId == id)
                .OrderByDescending(v => v.VersionNumber)
                .FirstOrDefaultAsync();

            int? coverMediaId = latestVersion?.CoverMediaId;

            if (request.CoverImage != null)
            {
                var uploadResult = await _mediaService.UploadAsync(
                    request.CoverImage,
                    userId);

                coverMediaId = uploadResult.MediaId;
            }

            // Create a new version
            var version = new NewsVersion
            {
                NewsId = news.NewsId,
                VersionNumber = (latestVersion?.VersionNumber ?? 0) + 1,

                Title = news.Title,
                Content = news.Content,

                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow,

                CoverMediaId = coverMediaId,

                ApprovalStatus = ApprovalStatus.Pending
            };

            _context.NewsVersions.Add(version);

            // Save both the updated News and the new NewsVersion
            await _context.SaveChangesAsync();

            var approvalRequest = new ApprovalRequest
            {
                NewsId = news.NewsId,

                RequestedBy = userId,
                RequestedAt = DateTime.UtcNow,

                Status = ApprovalStatus.Pending
            };

            _context.ApprovalRequests.Add(approvalRequest);

            await _context.SaveChangesAsync();

            version.ApprovalRequestId = approvalRequest.ApprovalRequestId;

            await _context.SaveChangesAsync();

            await _auditService.LogAsync(
                userId,
                "Update News",
                "News",
                news.NewsId);

            await transaction.CommitAsync();

            var creatorUsername = await _context.Users
                .Where(u => u.UserId == news.CreatedBy)
                .Select(u => u.Username)
                .FirstAsync();

            return new NewsResponse
            {
                NewsId = news.NewsId,
                Title = news.Title,
                Content = news.Content,
                ExpirationDate = news.ExpirationDate,
                CreatedAt = news.CreatedAt,
                CreatedBy = creatorUsername
            };
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}