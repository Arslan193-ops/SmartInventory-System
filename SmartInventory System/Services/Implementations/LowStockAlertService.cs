using Microsoft.EntityFrameworkCore;
using SmartInventory_System.Data;
using SmartInventory_System.Models.DTOs;
using SmartInventory_System.Models;
using SmartInventory_System.Services.Interfaces;

namespace SmartInventory_System.Services
{
    public class LowStockAlertService : ILowStockAlertService
    {
        private readonly ApplicationDbContext _context;

        public LowStockAlertService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LowStockAlertDto>> GetActiveAlertsAsync()
        {
            return await _context.LowStockAlerts
                .Include(a => a.Product)
                .Where(a => !a.IsResolved)
                .OrderBy(a => a.CreatedAt)
                .Select(a => new LowStockAlertDto
                {
                    Id = a.Id,
                    ProductId = a.ProductId,
                    ProductName = a.Product.Name,
                    CurrentQuantity = a.CurrentQuantity,
                    ReorderLevel = a.Product.ReorderLevel,
                    CreatedAt = a.CreatedAt,
                    IsResolved = a.IsResolved
                })
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
