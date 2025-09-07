using SmartInventory_System.Models.DTOs;

namespace SmartInventory_System.Services.Interfaces
{
    public interface ILowStockAlertService
    {
        Task<IEnumerable<LowStockAlertDto>> GetActiveAlertsAsync();
        Task ResolveAlertAsync(int alertId);
    }
}
