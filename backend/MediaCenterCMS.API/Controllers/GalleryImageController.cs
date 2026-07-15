using MediaCenterCMS.API.DTOs.GalleryImage;
using MediaCenterCMS.API.Enums;
using MediaCenterCMS.API.Services.GalleryImage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MediaCenterCMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GalleryImageController : ControllerBase
{
    private readonly IGalleryImageService _galleryImageService;

    public GalleryImageController(
        IGalleryImageService galleryImageService)
    {
        _galleryImageService = galleryImageService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromForm] CreateGalleryImageRequest request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
            return Unauthorized();

        var userId = int.Parse(userIdClaim.Value);

        var result = await _galleryImageService.CreateAsync(
            request,
            userId);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var images = await _galleryImageService.GetAllAsync();

        return Ok(images);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var image = await _galleryImageService.GetByIdAsync(id);

        if (image == null)
            return NotFound();

        return Ok(image);
    }

    [HttpPut("{id}/hide")]
    public async Task<IActionResult> Hide(int id)
    {
        var success = await _galleryImageService.SetVisibilityAsync(
            id,
            VisibilityStatus.Hidden);

        if (!success)
            return NotFound();

        return Ok();
    }

    [HttpPut("{id}/show")]
    public async Task<IActionResult> Show(int id)
    {
        var success = await _galleryImageService.SetVisibilityAsync(
            id,
            VisibilityStatus.Visible);

        if (!success)
            return NotFound();

        return Ok();
    }
}