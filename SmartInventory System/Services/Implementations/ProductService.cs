using Microsoft.EntityFrameworkCore;
using SmartInventory_System.Data;
using SmartInventory_System.Models;
using SmartInventory_System.Services.Interfaces;

namespace SmartInventory_System.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> UpdateAsync(int id, Product product)
        {
            var existing = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (existing == null) return null;

            existing.Name = product.Name;
            existing.Sku = product.Sku.ToUpperInvariant();
            existing.Barcode = product.Barcode;
            existing.Quantity = product.Quantity;
            existing.ReorderLevel = product.ReorderLevel;
            // Copy RowVersion for concurrency check
            existing.RowVersion = product.RowVersion;

            try
            {
                await _context.SaveChangesAsync();
                return existing;
            }
            catch (DbUpdateConcurrencyException)
            {
                // throw back so controller can handle it
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Products.FindAsync(id);
            if (existing == null) return false;

            _context.Products.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
