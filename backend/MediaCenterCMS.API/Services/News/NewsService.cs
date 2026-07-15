using MediaCenterCMS.API.Data;
using MediaCenterCMS.API.DTOs.News;
using MediaCenterCMS.API.Enums;
using MediaCenterCMS.API.Models;
using Microsoft.EntityFrameworkCore;
using NewsEntity = MediaCenterCMS.API.Models.News;

namespace MediaCenterCMS.API.Services.News;

public class NewsService : INewsService
{
    private readonly AppDbContext _context;

    public NewsService(AppDbContext context)
    {
        _context = context;
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

        var version = new NewsVersion
        {
            NewsId = news.NewsId,
            VersionNumber = 1,
            Title = news.Title,
            Content = news.Content,
            CreatedBy = userId,
            CreatedAt = DateTime.UtcNow,
            ApprovalStatus = ApprovalStatus.Pending
        };

        _context.NewsVersions.Add(version);
        await _context.SaveChangesAsync();

        await transaction.CommitAsync();

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
        .Select(n => new NewsResponse
        {
            NewsId = n.NewsId,
            Title = n.Title,
            Content = n.Content,
            ExpirationDate = n.ExpirationDate,
            CreatedAt = n.CreatedAt,
            CreatedBy = n.Creator.Username
        })
        .ToListAsync();
    }
    public async Task<NewsResponse?> GetByIdAsync(int id)
    {
        return await _context.News
            .Include(n => n.Creator)
            .Where(n => n.NewsId == id)
            .Select(n => new NewsResponse
            {
                NewsId = n.NewsId,
                Title = n.Title,
                Content = n.Content,
                ExpirationDate = n.ExpirationDate,
                CreatedAt = n.CreatedAt,
                CreatedBy = n.Creator.Username
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

            // Find the latest version number
            var latestVersion = await _context.NewsVersions
                .Where(v => v.NewsId == id)
                .MaxAsync(v => (int?)v.VersionNumber) ?? 0;

            // Create a new version
            var version = new NewsVersion
            {
                NewsId = news.NewsId,
                VersionNumber = latestVersion + 1,

                Title = news.Title,
                Content = news.Content,

                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow,

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