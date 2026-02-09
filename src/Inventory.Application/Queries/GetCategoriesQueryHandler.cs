using Inventory.Application.DTOs;
using Inventory.Application.Interfaces;

namespace Inventory.Application.Queries
{
    public sealed class GetCategoriesQueryHandler
    {
        private readonly ICategoryReadRepository categoryReadRepository;

        public GetCategoriesQueryHandler(ICategoryReadRepository categoryReadRepository)
        {
            this.categoryReadRepository = categoryReadRepository;
        }

        public async Task<IReadOnlyCollection<CategoryDto>> HandleAsync(GetCategoriesQuery query)
        {
            var categories = await categoryReadRepository.GetAllAsync();

            return categories
                .Select(category => new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name
                })
                .ToList();
        }
    }
}