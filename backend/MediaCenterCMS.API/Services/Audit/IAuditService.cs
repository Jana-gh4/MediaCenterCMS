using System.Threading.Tasks;

namespace MediaCenterCMS.API.Services.Audit;

public interface IAuditService
{
    Task LogAsync(
        int userId,
        string action,
        string entityName,
        int entityId);
}