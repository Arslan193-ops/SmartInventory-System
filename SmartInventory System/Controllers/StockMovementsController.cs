using Microsoft.AspNetCore.Mvc;
using SmartInventory_System.Models.DTOs;
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
        public async Task<IActionResult> AdjustStock([FromBody] StockMovementDto dto)
        {
            try
            {
                // Convert MovementType → qtyChange
                int qtyChange = dto.MovementType == MovementType.IN
                    ? dto.Quantity
                    : -dto.Quantity;

                var success = await _stockMovementService.RecordMovementAsync(dto.ProductId, qtyChange, dto.Note);

                if (!success) return NotFound("Product not found.");
                return Ok("Stock adjusted successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }


        // GET: api/StockMovements/1
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetMovements(
            int productId,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] MovementType? movementType,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var movements = await _stockMovementService.GetMovementsAsync(
                productId, from, to, movementType, page, pageSize);

            return Ok(movements);
        }

    }
}
