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
    public class CategoriesController : ControllerBase
    {
        private readonly GetCategoriesQueryHandler getCategoriesQueryHandler;
        private readonly CreateCategoryCommandHandler createCategoryCommandHandler;
        private readonly UpdateCategoryCommandHandler updateCategoryCommandHandler;
        private readonly DeleteCategoryCommandHandler deleteCategoryCommandHandler;
        private readonly GetCategoryByIdQueryHandler getCategoryById;

        public CategoriesController(GetCategoriesQueryHandler getCategoriesQueryHandler, CreateCategoryCommandHandler createCategoryCommandHandler, UpdateCategoryCommandHandler updateCategoryCommandHandler, DeleteCategoryCommandHandler deleteCategoryCommandHandler, GetCategoryByIdQueryHandler getCategoryById)
        {
            this.getCategoriesQueryHandler = getCategoriesQueryHandler;
            this.createCategoryCommandHandler = createCategoryCommandHandler;
            this.updateCategoryCommandHandler = updateCategoryCommandHandler;
            this.deleteCategoryCommandHandler = deleteCategoryCommandHandler;
            this.getCategoryById = getCategoryById;
        }

        /// <summary>Gets all categories</summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            Log.Information("Executing GetAllAsync");
            var result = await getCategoriesQueryHandler.HandleAsync(new GetCategoriesQuery());

            return Ok(result);
        }

        /// <summary>Creates a new category</summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateCategoryCommand command)
        {
            Log.Information("Creating category {CategoryName}", command.Name);
            await createCategoryCommandHandler.HandleAsync(command);

            return Created(string.Empty, null);
        }

        /// <summary>Updates an existing category</summary>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateCategoryCommand command)
        {
            Log.Information("Updating category {CategoryId}", command.Id);
            await updateCategoryCommandHandler.HandleAsync(command);

            return Ok();
        }

        /// <summary>Deletes a category</summary>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteCategoryCommand command)
        {
            Log.Information("Deleting category {CategoryId}", command.Id);
            await deleteCategoryCommandHandler.HandleAsync(command);

            return Ok();
        }

        /// <summary>Gets a category by Id</summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            Log.Information("Getting category by Id {CategoryId}", id);
            var category = await getCategoryById.HandleAsync(new GetCategoryByIdQuery(id));

            return Ok(category);
        }
    }
}