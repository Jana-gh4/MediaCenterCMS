using MediaCenterCMS.API.Data;
using MediaCenterCMS.API.Models;
using MediaCenterCMS.API.DTOs.Audit;
using Microsoft.EntityFrameworkCore;

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
    public async Task<IEnumerable<AuditLogResponse>> GetAllAsync()
    {
        return await _context.AuditLogs
            .Include(a => a.User)
            .OrderByDescending(a => a.Timestamp)
            .Select(a => new AuditLogResponse
            {
                AuditLogId = a.AuditLogId,
                Username = a.User.Username,
                Action = a.Action,
                EntityName = a.EntityName,
                EntityId = a.EntityId,
                Timestamp = a.Timestamp
            })
            .ToListAsync();
    }
}