using MediaCenterCMS.API.DTOs.Approval;
using MediaCenterCMS.API.DTOs.News;

namespace MediaCenterCMS.API.Services.Approval;

public interface IApprovalService
{
    Task<IEnumerable<ApprovalResponse>> GetPendingAsync();

    Task<NewsResponse?> ApproveAsync(
        int approvalRequestId,
        int reviewerId);

    Task<bool> RejectAsync(
        int approvalRequestId,
        int reviewerId,
        string reason);
}