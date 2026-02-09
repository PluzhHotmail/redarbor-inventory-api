using Inventory.Application.Commands;
using Inventory.Application.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    [Authorize]
    public sealed class ProductsController : ControllerBase
    {
        private readonly GetProductsQueryHandler getProductsQueryHandler;
        private readonly CreateProductCommandHandler createProductCommandHandler;
        private readonly UpdateProductCommandHandler updateProductCommandHandler;
        private readonly DeleteProductCommandHandler deleteProductCommandHandler;
        private readonly GetProductByIdQueryHandler _getProductById;

        public ProductsController(GetProductsQueryHandler getProductsQueryHandler, CreateProductCommandHandler createProductCommandHandler, UpdateProductCommandHandler updateProductCommandHandler, DeleteProductCommandHandler deleteProductCommandHandler, GetProductByIdQueryHandler getProductById)
        {
            this.getProductsQueryHandler = getProductsQueryHandler;
            this.createProductCommandHandler = createProductCommandHandler;
            this.updateProductCommandHandler = updateProductCommandHandler;
            this.deleteProductCommandHandler = deleteProductCommandHandler;
            this._getProductById = getProductById;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await getProductsQueryHandler.HandleAsync(new GetProductsQuery());

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateProductCommand command)
        {
            await createProductCommandHandler.HandleAsync(command);

            return Created(string.Empty, null);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateProductCommand command)
        {
            await updateProductCommandHandler.HandleAsync(command);

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteProductCommand command)
        {
            await deleteProductCommandHandler.HandleAsync(command);

            return NoContent();
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _getProductById.HandleAsync(new GetProductByIdQuery(id));
            if (product == null)
                return NotFound();

            return Ok(product);
        }

    }
}