using Inventory.Application.DTOs;
using Inventory.Application.Interfaces;

namespace Inventory.Application.Queries
{
    public sealed class GetProductsQueryHandler
    {
        private readonly IProductReadRepository productReadRepository;

        public GetProductsQueryHandler(IProductReadRepository productReadRepository)
        {
            this.productReadRepository = productReadRepository;
        }

        public async Task<IReadOnlyCollection<ProductDto>> HandleAsync(GetProductsQuery query, CancellationToken cancellationToken = default)
        {
            var products = await productReadRepository.GetAllAsync();

            return products
                .Select(product => new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Stock = product.Stock
                })
                .ToList();
        }
    }
}