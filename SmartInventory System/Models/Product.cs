using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartInventory_System.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, MaxLength(250)]
        public string Name { get; set; } = null!;

        [Required, MaxLength(50)]
        public string Sku { get; set; } = null!; // unique, normalized (we'll uppercase before saving)

        [MaxLength(100)]
        public string? Barcode { get; set; }

        
        public int Quantity { get; set; } = 0;

        public int ReorderLevel { get; set; } = 0;

        // RowVersion for optimistic concurrency
        [Timestamp]
        public byte[]? RowVersion { get; set; }
    }
}


