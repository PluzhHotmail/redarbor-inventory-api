using Inventory.Application.Commands;
using Inventory.Application.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/products")]
[Authorize]
public sealed class ProductsController : ControllerBase
{
    private readonly GetProductsQueryHandler getProductsQueryHandler;
    private readonly CreateProductCommandHandler createProductCommandHandler;

    public ProductsController(
        GetProductsQueryHandler getProductsQueryHandler,
        CreateProductCommandHandler createProductCommandHandler)
    {
        this.getProductsQueryHandler = getProductsQueryHandler;
        this.createProductCommandHandler = createProductCommandHandler;
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
}