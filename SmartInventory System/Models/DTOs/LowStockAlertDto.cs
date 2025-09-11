namespace SmartInventory_System.Models.DTOs
{
    public class LowStockAlertDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CurrentQuantity { get; set; }
        public int ReorderLevel { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsResolved { get; set; }
    }
}
