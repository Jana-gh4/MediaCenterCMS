using MediaCenterCMS.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MediaCenterCMS.API.Data;

namespace MediaCenterCMS.API.Services;

public class DatabaseSeeder
{
    private readonly AppDbContext _context;

    public DatabaseSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        // Don't create another admin if one already exists
        if (await _context.Users.AnyAsync(u => u.Username == "admin"))
            return;

        var passwordHasher = new PasswordHasher<User>();

        var admin = new User
        {
            Username = "admin",
            FullName = "System Administrator",
            RoleId = 1,
            IsActive = true
        };

        admin.PasswordHash = passwordHasher.HashPassword(admin, "Admin@123");
        _context.Users.Add(admin);

        await _context.SaveChangesAsync();
    }
}