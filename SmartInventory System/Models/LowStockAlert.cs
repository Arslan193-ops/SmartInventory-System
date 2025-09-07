using System.ComponentModel.DataAnnotations.Schema;

namespace SmartInventory_System.Models
{
    public class LowStockAlert
    {
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public int CurrentQuantity { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsResolved { get; set; } = false;

        public Product Product { get; set; }  // Navigation property
    }
}
