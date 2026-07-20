namespace MediaCenterCMS.API.DTOs.Approval;

public class ApprovalResponse
{
    public int ApprovalRequestId { get; set; }

    public string EntityType { get; set; } = string.Empty;

    public int EntityId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string RequestedBy { get; set; } = string.Empty;

    public DateTime RequestedAt { get; set; }

    public string Status { get; set; } = string.Empty;
}