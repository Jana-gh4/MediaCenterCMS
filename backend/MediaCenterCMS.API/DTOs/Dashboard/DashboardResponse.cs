namespace MediaCenterCMS.API.DTOs.Dashboard;

public class DashboardResponse
{
    public int NewsCount { get; set; }

    public int ImagesCount { get; set; }

    public int VideosCount { get; set; }

    public int PendingApprovals { get; set; }
}