using System.ComponentModel.DataAnnotations;

namespace SmartInventory_System.DTOs
{
    public class StockMovementDto
    {
        public int ProductId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        public int Quantity { get; set; }  // Always positive

        [Required]
        public MovementType MovementType { get; set; }

        public string Note { get; set; }
    }

    public enum MovementType
    {
        IN = 1,
        OUT = 2
    }

}
