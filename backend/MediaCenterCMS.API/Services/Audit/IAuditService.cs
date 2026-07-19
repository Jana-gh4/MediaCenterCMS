using MediaCenterCMS.API.DTOs.Audit;

namespace MediaCenterCMS.API.Services.Audit;

public interface IAuditService
{
    Task LogAsync(
        int userId,
        string action,
        string entityName,
        int entityId);

    Task<IEnumerable<AuditLogResponse>> GetAllAsync();
}