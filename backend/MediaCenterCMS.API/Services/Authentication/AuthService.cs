using MediaCenterCMS.API.Data;
using MediaCenterCMS.API.DTOs;
using MediaCenterCMS.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MediaCenterCMS.API.Services.Authentication;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IJwtService _jwtService;

    public AuthService(
        AppDbContext context,
        IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
    var user = await _context.Users
        .Include(u => u.Role)
        .FirstOrDefaultAsync(u => u.Username == request.Username);

    if (user == null)
        return null;
    
    var passwordHasher = new PasswordHasher<User>();

    var verificationResult = passwordHasher.VerifyHashedPassword(
        user,
        user.PasswordHash,
        request.Password);

    if (verificationResult == PasswordVerificationResult.Failed)
        return null;
    
    if (!user.IsActive)
        return null;
    
    user.LastLoginAt = DateTime.UtcNow;

    await _context.SaveChangesAsync();

    var token = _jwtService.GenerateToken(user);

    return new LoginResponse
    {
        Token = token,
        Username = user.Username,
        Role = user.Role.Name
    };
    }
}