using MediaCenterCMS.API.Data;
using MediaCenterCMS.API.Models;

namespace MediaCenterCMS.API.Services.Audit;

public class AuditService : IAuditService
{
    private readonly AppDbContext _context;

    public AuditService(AppDbContext context)
    {
        _context = context;
    }

    public async Task LogAsync(
        int userId,
        string action,
        string entityName,
        int entityId)
    {
        var log = new AuditLog
        {
            UserId = userId,
            Action = action,
            EntityName = entityName,
            EntityId = entityId,
            Timestamp = DateTime.UtcNow
        };

        _context.AuditLogs.Add(log);

        await _context.SaveChangesAsync();
    }
}