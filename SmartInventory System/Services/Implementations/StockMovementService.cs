using Microsoft.EntityFrameworkCore;
using SmartInventory_System.Data;
using SmartInventory_System.Models;
using SmartInventory_System.Services.Interfaces;

namespace SmartInventory_System.Services
{
    public class StockMovementService : IStockMovementService
    {
        private readonly ApplicationDbContext _context;

        public StockMovementService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RecordMovementAsync(int productId, int qtyChange, string note)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
                if (product == null) return false;

                if (product.Quantity + qtyChange < 0)
                    throw new InvalidOperationException("Stock cannot go negative.");

                // Update product stock
                product.Quantity += qtyChange;

                // Save movement
                var movement = new StockMovement
                {
                    ProductId = product.Id,
                    QuantityChange = qtyChange,
                    Note = note,
                    CreatedAt = DateTime.UtcNow
                };

                _context.StockMovements.Add(movement);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<StockMovement>> GetMovementsAsync(int productId)
        {
            return await _context.StockMovements
                .Where(sm => sm.ProductId == productId)
                .OrderByDescending(sm => sm.CreatedAt)
                .ToListAsync();
        }
    }
}
