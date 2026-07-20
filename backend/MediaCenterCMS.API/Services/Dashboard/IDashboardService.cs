using MediaCenterCMS.API.DTOs.Dashboard;

namespace MediaCenterCMS.API.Services.Dashboard;

public interface IDashboardService
{
    Task<DashboardResponse> GetDashboardAsync();
}