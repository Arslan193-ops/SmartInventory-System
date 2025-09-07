using SmartInventory_System.Models;

namespace SmartInventory_System.Services.Interfaces
{
    public interface ILowStockAlertService
    {
        Task<IEnumerable<LowStockAlert>> GetActiveAlertsAsync();
        Task ResolveAlertAsync(int alertId);
    }
}
