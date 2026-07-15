using MediaCenterCMS.API.DTOs.News;

namespace MediaCenterCMS.API.Services.Approval;

public interface IApprovalService
{
    Task<NewsResponse?> ApproveAsync(
        int approvalRequestId,
        int reviewerId);

    Task<bool> RejectAsync(
        int approvalRequestId,
        int reviewerId,
        string reason);
}