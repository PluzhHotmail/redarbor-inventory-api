using Inventory.Application.Commands;
using Inventory.Application.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Inventory.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public sealed class ProductsController : ControllerBase
    {
        private readonly GetProductsQueryHandler getProductsQueryHandler;
        private readonly CreateProductCommandHandler createProductCommandHandler;
        private readonly UpdateProductCommandHandler updateProductCommandHandler;
        private readonly DeleteProductCommandHandler deleteProductCommandHandler;
        private readonly GetProductByIdQueryHandler getProductById;

        public ProductsController(
            GetProductsQueryHandler getProductsQueryHandler,
            CreateProductCommandHandler createProductCommandHandler,
            UpdateProductCommandHandler updateProductCommandHandler,
            DeleteProductCommandHandler deleteProductCommandHandler,
            GetProductByIdQueryHandler getProductById)
        {
            this.getProductsQueryHandler = getProductsQueryHandler;
            this.createProductCommandHandler = createProductCommandHandler;
            this.updateProductCommandHandler = updateProductCommandHandler;
            this.deleteProductCommandHandler = deleteProductCommandHandler;
            this.getProductById = getProductById;
        }

        /// <summary>Gets all products</summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            Log.Information("Executing GetAllAsync");
            var products = await getProductsQueryHandler.HandleAsync(new GetProductsQuery());

            return Ok(products);
        }

        /// <summary>Creates a new product</summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateProductCommand command)
        {
            Log.Information("Creating product {ProductName} in category {CategoryId}", command.Name, command.CategoryId);
            await createProductCommandHandler.HandleAsync(command);

            return Created(string.Empty, null);
        }

        /// <summary>Updates an existing product</summary>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateProductCommand command)
        {
            Log.Information("Updating product {ProductId}", command.Id);
            await updateProductCommandHandler.HandleAsync(command);

            return Ok();
        }

        /// <summary>Deletes a product</summary>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteProductCommand command)
        {
            Log.Information("Deleting product {ProductId}", command.Id);
            await deleteProductCommandHandler.HandleAsync(command);

            return Ok();
        }

        /// <summary>Gets a product by Id</summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            Log.Information("Getting product by Id {ProductId}", id);
            var product = await getProductById.HandleAsync(new GetProductByIdQuery(id));

            return Ok(product);
        }
    }
}