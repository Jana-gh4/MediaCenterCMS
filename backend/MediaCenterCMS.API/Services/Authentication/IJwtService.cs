using MediaCenterCMS.API.Models;

namespace MediaCenterCMS.API.Services.Authentication;

public interface IJwtService
{
    string GenerateToken(User user);
}