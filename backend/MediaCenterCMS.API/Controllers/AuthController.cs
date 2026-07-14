using MediaCenterCMS.API.DTOs;
using MediaCenterCMS.API.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace MediaCenterCMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);

        if (result == null)
            return Unauthorized("Invalid username or password.");

        return Ok(result);
    }
}
