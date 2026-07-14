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

        return new NewsResponse
        {
            NewsId = news.NewsId,
            Title = news.Title,
            Content = news.Content,
            ExpirationDate = news.ExpirationDate,
            CreatedAt = news.CreatedAt,
            CreatedBy = string.Empty
        };
    }
}