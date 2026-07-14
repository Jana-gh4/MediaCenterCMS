using MediaCenterCMS.API.DTOs;

namespace MediaCenterCMS.API.Services.Authentication;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
}