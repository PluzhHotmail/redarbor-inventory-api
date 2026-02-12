using Inventory.Application.Commands;
using Inventory.Application.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        public ProductsController(GetProductsQueryHandler getProductsQueryHandler, CreateProductCommandHandler createProductCommandHandler, UpdateProductCommandHandler updateProductCommandHandler, DeleteProductCommandHandler deleteProductCommandHandler, GetProductByIdQueryHandler getProductById)
        {
            this.getProductsQueryHandler = getProductsQueryHandler;
            this.createProductCommandHandler = createProductCommandHandler;
            this.updateProductCommandHandler = updateProductCommandHandler;
            this.deleteProductCommandHandler = deleteProductCommandHandler;
            this.getProductById = getProductById;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var products = await getProductsQueryHandler.HandleAsync(new GetProductsQuery());

            return Ok(products);
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

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteProductCommand command)
        {
            await deleteProductCommandHandler.HandleAsync(command);

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await getProductById.HandleAsync(new GetProductByIdQuery(id));

            return Ok(product);
        }
    }
}