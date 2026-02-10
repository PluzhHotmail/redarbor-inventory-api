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
            if (command.Quantity <= 0)
            {
                throw new InvalidOperationException("Quantity must be greater than zero");
            }
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

            product.Stock = CalculateNewStock(product, command);

            await productWriteRepository.UpdateStockAsync(product);
        }

        private static int CalculateNewStock(Product product, RegisterInventoryMovementCommand command)
        {
            return command.Type == InventoryMovementType.Entry ? product.Stock + command.Quantity : product.Stock - command.Quantity;
        }
    }
}