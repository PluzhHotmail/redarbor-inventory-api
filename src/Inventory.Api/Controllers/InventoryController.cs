using Inventory.Application.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Inventory.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly RegisterInventoryMovementCommandHandler registerInventoryMovementCommandHandler;
        public InventoryController(RegisterInventoryMovementCommandHandler registerInventoryMovementCommandHandler)
        {
            this.registerInventoryMovementCommandHandler = registerInventoryMovementCommandHandler;
        }

        /// <summary>Record inventory movement</summary>
        [HttpPost("movements")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RegisterMovement([FromBody] RegisterInventoryMovementCommand command)
        {
            Log.Information("Recording movement for the product {ProductId}", command.ProductId);
            await registerInventoryMovementCommandHandler.HandleAsync(command);

            return Ok();
        }
    }
}