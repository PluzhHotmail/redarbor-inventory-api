using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;

namespace Inventory.Application.Queries;

public sealed class GetProductByIdQueryHandler
{
    private readonly IProductReadRepository _repository;

    public GetProductByIdQueryHandler(IProductReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<Product?> HandleAsync(GetProductByIdQuery query)
    {
        return await _repository.GetByIdAsync(query.Id);
    }
}