using MediaCenterCMS.API.Configuration;
using MediaCenterCMS.API.Models;
using Microsoft.Extensions.Options;

namespace MediaCenterCMS.API.Services.Authentication;

public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;

    public JwtService(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }

    public string GenerateToken(User user)
    {
        throw new NotImplementedException();
    }
}