namespace MediaCenterCMS.API.Models;

public class User
{
    public int UserId { get; set; }

    public int RoleId { get; set; }

    public string Username { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    // Navigation Property
    public Role Role { get; set; } = null!;
}