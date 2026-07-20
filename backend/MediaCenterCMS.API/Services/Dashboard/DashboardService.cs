using MediaCenterCMS.API.Data;
using MediaCenterCMS.API.DTOs.Dashboard;
using MediaCenterCMS.API.Enums;
using Microsoft.EntityFrameworkCore;

namespace MediaCenterCMS.API.Services.Dashboard;

public class DashboardService : IDashboardService
{
    private readonly AppDbContext _context;

    public DashboardService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardResponse> GetDashboardAsync()
    {
        return new DashboardResponse
        {
            NewsCount = await _context.News.CountAsync(),

            ImagesCount = await _context.GalleryImages.CountAsync(),

            VideosCount = await _context.GalleryVideos.CountAsync(),

            PendingApprovals = await _context.ApprovalRequests
                .CountAsync(a => a.Status == ApprovalStatus.Pending)
        };
    }
}