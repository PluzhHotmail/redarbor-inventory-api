using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;

namespace Inventory.Application.Commands
{
    public sealed class RegisterInventoryMovementCommandHandler
    {
        private readonly IProductReadRepository productReadRepository;
        private readonly IProductWriteRepository productWriteRepository;
        private readonly IInventoryMovementWriteRepository inventoryMovementWriteRepository;

        public RegisterInventoryMovementCommandHandler(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, IInventoryMovementWriteRepository inventoryMovementWriteRepository)
        {
            this.productReadRepository = productReadRepository;
            this.productWriteRepository = productWriteRepository;
            this.inventoryMovementWriteRepository = inventoryMovementWriteRepository;
        }

        public async Task HandleAsync(RegisterInventoryMovementCommand command)
        {
            var product = await productReadRepository.GetByIdAsync(command.ProductId);
            if (product is null)
            {
                throw new InvalidOperationException("Product not found");
            }
            if (command.Type == InventoryMovementType.Exit && product.Stock < command.Quantity)
            {
                throw new InvalidOperationException("Insufficient stock");
            }
            var movement = new InventoryMovement(
                Guid.NewGuid(),
                command.ProductId,
                command.Quantity,
                command.Type
            );

            await inventoryMovementWriteRepository.AddAsync(movement);

            var newStock = command.Type == InventoryMovementType.Entry
                ? product.Stock + command.Quantity
                : product.Stock - command.Quantity;
            product.Stock = newStock;

            await productWriteRepository.UpdateStockAsync(product);
        }
    }
}