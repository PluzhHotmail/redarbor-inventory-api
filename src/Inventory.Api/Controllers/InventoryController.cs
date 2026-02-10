using Inventory.Application.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers
{
    [ApiController]
    [Route("api/inventory")]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly RegisterInventoryMovementCommandHandler registerInventoryMovementCommandHandler;
        public InventoryController(RegisterInventoryMovementCommandHandler registerInventoryMovementCommandHandler)
        {
            this.registerInventoryMovementCommandHandler = registerInventoryMovementCommandHandler;
        }

        [HttpPost("movements")]
        public async Task<IActionResult> RegisterMovement([FromBody] RegisterInventoryMovementCommand command)
        {
            await registerInventoryMovementCommandHandler.HandleAsync(command);

            return Ok();
        }
    }
}