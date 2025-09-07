using Microsoft.EntityFrameworkCore;
using SmartInventory_System.Data;
using SmartInventory_System.Models;
using SmartInventory_System.Services.Interfaces;

namespace SmartInventory_System.Services.Implementations
{
    public class LowStockAlertService : ILowStockAlertService
    {
        private readonly ApplicationDbContext _context;

        public LowStockAlertService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LowStockAlert>> GetActiveAlertsAsync()
        {
            return await _context.LowStockAlerts
                .Include(a => a.Product) // so you can see product info in the alert
                .Where(a => !a.IsResolved)
                .OrderBy(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task ResolveAlertAsync(int alertId)
        {
            var alert = await _context.LowStockAlerts.FirstOrDefaultAsync(a => a.Id == alertId);
            if (alert != null)
            {
                alert.IsResolved = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
