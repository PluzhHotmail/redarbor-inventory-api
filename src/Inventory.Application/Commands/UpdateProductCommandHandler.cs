using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;

namespace Inventory.Application.Commands;

public sealed class UpdateProductCommandHandler
{
    private readonly IProductWriteRepository productWriteRepository;

    public UpdateProductCommandHandler(IProductWriteRepository productWriteRepository)
    {
        this.productWriteRepository = productWriteRepository;
    }

    public async Task HandleAsync(UpdateProductCommand command)
    {
        var product = new Product(
            command.Id,
            command.Name,
            command.Stock,
            command.CategoryId);

        await productWriteRepository.UpdateAsync(product);
    }
}