using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;

namespace Inventory.Application.Commands;

public sealed class CreateProductCommandHandler
{
    private readonly IProductWriteRepository productWriteRepository;

    public CreateProductCommandHandler(IProductWriteRepository productWriteRepository)
    {
        this.productWriteRepository = productWriteRepository;
    }

    public async Task HandleAsync(CreateProductCommand command)
    {
        var product = new Product(
            command.Id,
            command.Name,
            command.CategoryId);

        await productWriteRepository.CreateAsync(product);
    }
}