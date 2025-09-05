using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartInventory_System.Models
{
    public class StockMovement
    {
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public int QuantityChange { get; set; }

        public string Note { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Product Product { get; set; }  // Navigation property
    }
}
