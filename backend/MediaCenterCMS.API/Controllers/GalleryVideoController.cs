using MediaCenterCMS.API.DTOs.GalleryVideo;
using MediaCenterCMS.API.Enums;
using MediaCenterCMS.API.Services.GalleryVideo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MediaCenterCMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GalleryVideoController : ControllerBase
{
    private readonly IGalleryVideoService _galleryVideoService;

    public GalleryVideoController(
        IGalleryVideoService galleryVideoService)
    {
        _galleryVideoService = galleryVideoService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromForm] CreateGalleryVideoRequest request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
            return Unauthorized();

        var userId = int.Parse(userIdClaim.Value);

        var result = await _galleryVideoService.CreateAsync(
            request,
            userId);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var videos = await _galleryVideoService.GetAllAsync();

        return Ok(videos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var video = await _galleryVideoService.GetByIdAsync(id);

        if (video == null)
            return NotFound();

        return Ok(video);
    }

    [HttpPut("{id}/hide")]
    public async Task<IActionResult> Hide(int id)
    {
        var success = await _galleryVideoService.SetVisibilityAsync(
            id,
            VisibilityStatus.Hidden);

        if (!success)
            return NotFound();

        return Ok();
    }

    [HttpPut("{id}/show")]
    public async Task<IActionResult> Show(int id)
    {
        var success = await _galleryVideoService.SetVisibilityAsync(
            id,
            VisibilityStatus.Visible);

        if (!success)
            return NotFound();

        return Ok();
    }
}