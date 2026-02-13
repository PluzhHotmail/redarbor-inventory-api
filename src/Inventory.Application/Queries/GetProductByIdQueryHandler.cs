using Inventory.Application.Exceptions;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;

namespace Inventory.Application.Queries
{
    public sealed class GetProductByIdQueryHandler
    {
        private readonly IProductReadRepository _repository;

        public GetProductByIdQueryHandler(IProductReadRepository repository)
        {
            _repository = repository;
        }

        public async Task<Product?> HandleAsync(GetProductByIdQuery query, CancellationToken cancellationToken = default)
        {
            var product = await _repository.GetByIdAsync(query.Id);
            if (product is null)
            {
                throw new NotFoundException("Product not found.");
            }

            return product;
        }
    }
}