using InventarisKKP.Models;

namespace InventarisKKP.Services
{
    /// <summary>
    /// Interface untuk service ActivityLog MongoDB
    /// </summary>
    public interface IActivityLogService
    {
        Task LogActivityAsync(string user, string action, string description);
        Task<List<ActivityLog>> GetLogsAsync(int limit = 100);
    }
}