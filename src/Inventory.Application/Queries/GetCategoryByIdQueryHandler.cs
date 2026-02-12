using Inventory.Application.Exceptions;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;

namespace Inventory.Application.Queries
{
    public sealed class GetCategoryByIdQueryHandler
    {
        private readonly ICategoryReadRepository _repository;

        public GetCategoryByIdQueryHandler(ICategoryReadRepository repository)
        {
            _repository = repository;
        }

        public async Task<Category?> HandleAsync(GetCategoryByIdQuery query)
        {
            var category = await _repository.GetByIdAsync(query.Id);
            if (category is null)
            {
                throw new NotFoundException("Category not found.");
            }

            return category;
        }
    }
}