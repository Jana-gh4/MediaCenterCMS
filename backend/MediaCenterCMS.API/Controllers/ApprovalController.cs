using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MediaCenterCMS.API.Services.Approval;
using MediaCenterCMS.API.DTOs.Approval;

namespace MediaCenterCMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class ApprovalController : ControllerBase
{
    private readonly IApprovalService _approvalService;

    public ApprovalController(IApprovalService approvalService)
    {
        _approvalService = approvalService;
    }

    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(int id)
    {
        var reviewerId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await _approvalService.ApproveAsync(id, reviewerId);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost("{id}/reject")]
    public async Task<IActionResult> Reject(
        int id,
        RejectRequest request)
    {
        var reviewerId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var success = await _approvalService
            .RejectAsync(id, reviewerId, request.Reason);

        if (!success)
            return NotFound();

        return Ok();
    }
}