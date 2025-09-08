using SmartInventory_System.Models;

namespace SmartInventory_System.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product> AddAsync(Product product);
        Task<Product?> UpdateAsync(int id, Product product);
        Task<bool> DeleteAsync(int id);
        Task<bool> AdjustStockAsync(int productId, int qtyChange, string note);
        Task<Product?> GetByBarcodeAsync(string barcode);



    }
}
