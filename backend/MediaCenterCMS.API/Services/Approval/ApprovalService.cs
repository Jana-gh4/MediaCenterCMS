using MediaCenterCMS.API.Data;
using MediaCenterCMS.API.DTOs.News;
using Microsoft.EntityFrameworkCore;
using MediaCenterCMS.API.Enums;


namespace MediaCenterCMS.API.Services.Approval;

public class ApprovalService : IApprovalService
{
    private readonly AppDbContext _context;

    public ApprovalService(AppDbContext context)
    {
        _context = context;
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

            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}