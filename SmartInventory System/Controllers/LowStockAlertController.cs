using Microsoft.AspNetCore.Mvc;
using SmartInventory_System.Services.Interfaces;

namespace SmartInventory_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LowStockAlertsController : ControllerBase
    {
        private readonly ILowStockAlertService _alertService;

        public LowStockAlertsController(ILowStockAlertService alertService)
        {
            _alertService = alertService;
        }

        // GET: api/LowStockAlerts
        [HttpGet]
        public async Task<IActionResult> GetActiveAlerts()
        {
            var alerts = await _alertService.GetActiveAlertsAsync();
            return Ok(alerts);
        }

        // PUT: api/LowStockAlerts/5/resolve
        [HttpPut("{id}/resolve")]
        public async Task<IActionResult> ResolveAlert(int id)
        {
            await _alertService.ResolveAlertAsync(id);
            return Ok("Alert resolved.");
        }
    }
}
