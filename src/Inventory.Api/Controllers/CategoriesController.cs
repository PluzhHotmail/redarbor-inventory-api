using Inventory.Application.Commands;
using Inventory.Application.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers
{
    [ApiController]
    [Route("api/categories")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly GetCategoriesQueryHandler getCategoriesQueryHandler;
        private readonly CreateCategoryCommandHandler createCategoryCommandHandler;
        private readonly UpdateCategoryCommandHandler updateCategoryCommandHandler;
        private readonly DeleteCategoryCommandHandler deleteCategoryCommandHandler;

        public CategoriesController(GetCategoriesQueryHandler getCategoriesQueryHandler, CreateCategoryCommandHandler createCategoryCommandHandler, UpdateCategoryCommandHandler updateCategoryCommandHandler, DeleteCategoryCommandHandler deleteCategoryCommandHandler)
        {
            this.getCategoriesQueryHandler = getCategoriesQueryHandler;
            this.createCategoryCommandHandler = createCategoryCommandHandler;
            this.updateCategoryCommandHandler = updateCategoryCommandHandler;
            this.deleteCategoryCommandHandler = deleteCategoryCommandHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await getCategoriesQueryHandler.HandleAsync(new GetCategoriesQuery());

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateCategoryCommand command)
        {
            await createCategoryCommandHandler.HandleAsync(command);

            return Created(string.Empty, null);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateCategoryCommand command)
        {
            await updateCategoryCommandHandler.HandleAsync(command);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteCategoryCommand command)
        {
            await deleteCategoryCommandHandler.HandleAsync(command);

            return Ok();
        }
    }
}