using Microsoft.AspNetCore.Mvc;
using SmartInventory_System.Services.Interfaces;

namespace SmartInventory_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockMovementsController : ControllerBase
    {
        private readonly IStockMovementService _stockMovementService;

        public StockMovementsController(IStockMovementService stockMovementService)
        {
            _stockMovementService = stockMovementService;
        }

        // POST: api/StockMovements/adjust?productId=1&qtyChange=-2&note=Sale
        [HttpPost("adjust")]
        public async Task<IActionResult> AdjustStock(int productId, int qtyChange, string note)
        {
            try
            {
                var success = await _stockMovementService.RecordMovementAsync(productId, qtyChange, note);
                if (!success) return NotFound("Product not found.");
                return Ok("Stock adjusted successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/StockMovements/1
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetMovements(int productId)
        {
            var movements = await _stockMovementService.GetMovementsAsync(productId);
            return Ok(movements);
        }
    }
}
