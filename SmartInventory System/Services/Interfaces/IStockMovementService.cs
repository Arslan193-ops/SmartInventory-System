using SmartInventory_System.Models;

namespace SmartInventory_System.Services.Interfaces
{
    public interface IStockMovementService
    {
        Task<bool> RecordMovementAsync(int productId, int qtyChange, string note);
        Task<IEnumerable<StockMovement>> GetMovementsAsync(int productId);
    }
}
