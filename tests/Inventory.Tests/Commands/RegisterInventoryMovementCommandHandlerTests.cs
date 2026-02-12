using FluentValidation;
using Inventory.Application.Commands;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Tests.Commands
{
    public sealed class RegisterInventoryMovementCommandHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldThrow_WhenQuantityIsLessOrEqualThanZero()
        {
            var productReadRepository = new Mock<IProductReadRepository>();
            var productWriteRepository = new Mock<IProductWriteRepository>();
            var inventoryMovementWriteRepository = new Mock<IInventoryMovementWriteRepository>();
            var validator = new Mock<IValidator<RegisterInventoryMovementCommand>>();
            var handler = new RegisterInventoryMovementCommandHandler(
                productReadRepository.Object,
                productWriteRepository.Object,
                inventoryMovementWriteRepository.Object,
                validator.Object
            );
            var command = new RegisterInventoryMovementCommand
            {
                ProductId = Guid.NewGuid(),
                Quantity = 0,
                Type = InventoryMovementType.Entry
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));
        }

        [Fact]
        public async Task HandleAsync_ShouldThrow_WhenProductDoesNotExist()
        {
            var productReadRepository = new Mock<IProductReadRepository>();
            productReadRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Product)null);
            var productWriteRepository = new Mock<IProductWriteRepository>();
            var inventoryMovementWriteRepository = new Mock<IInventoryMovementWriteRepository>();
            var validator = new Mock<IValidator<RegisterInventoryMovementCommand>>();
            var handler = new RegisterInventoryMovementCommandHandler(
                productReadRepository.Object,
                productWriteRepository.Object,
                inventoryMovementWriteRepository.Object,
                validator.Object
            );
            var command = new RegisterInventoryMovementCommand
            {
                ProductId = Guid.NewGuid(),
                Quantity = 5,
                Type = InventoryMovementType.Entry
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));
        }

        [Fact]
        public async Task HandleAsync_ShouldThrow_WhenExitMovementAndInsufficientStock()
        {
            var product = new Product(
                Guid.NewGuid(),
                "Keyboard",
                stock: 2,
                Guid.NewGuid()
            );
            var productReadRepository = new Mock<IProductReadRepository>();
            productReadRepository
                .Setup(r => r.GetByIdAsync(product.Id))
                .ReturnsAsync(product);
            var productWriteRepository = new Mock<IProductWriteRepository>();
            var inventoryMovementWriteRepository = new Mock<IInventoryMovementWriteRepository>();
            var validator = new Mock<IValidator<RegisterInventoryMovementCommand>>();
            var handler = new RegisterInventoryMovementCommandHandler(
                productReadRepository.Object,
                productWriteRepository.Object,
                inventoryMovementWriteRepository.Object,
                validator.Object
            );
            var command = new RegisterInventoryMovementCommand
            {
                ProductId = product.Id,
                Quantity = 5,
                Type = InventoryMovementType.Exit
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));
        }

        [Fact]
        public async Task HandleAsync_ShouldIncreaseStock_WhenEntryMovement()
        {
            var product = new Product(
                Guid.NewGuid(),
                "Monitor",
                stock: 5,
                Guid.NewGuid()
            );
            var productReadRepository = new Mock<IProductReadRepository>();
            productReadRepository
                .Setup(r => r.GetByIdAsync(product.Id))
                .ReturnsAsync(product);
            var productWriteRepository = new Mock<IProductWriteRepository>();
            var inventoryMovementWriteRepository = new Mock<IInventoryMovementWriteRepository>();
            var validator = new Mock<IValidator<RegisterInventoryMovementCommand>>();
            var handler = new RegisterInventoryMovementCommandHandler(
                productReadRepository.Object,
                productWriteRepository.Object,
                inventoryMovementWriteRepository.Object,
                validator.Object
            );
            var command = new RegisterInventoryMovementCommand
            {
                ProductId = product.Id,
                Quantity = 3,
                Type = InventoryMovementType.Entry
            };

            await handler.HandleAsync(command);

            Assert.Equal(8, product.Stock);
            inventoryMovementWriteRepository.Verify(
                r => r.AddAsync(It.IsAny<InventoryMovement>()),
                Times.Once
            );
            productWriteRepository.Verify(
                r => r.UpdateStockAsync(product),
                Times.Once
            );
        }

        [Fact]
        public async Task HandleAsync_ShouldDecreaseStock_WhenExitMovement()
        {
            var product = new Product(
                Guid.NewGuid(),
                "Mouse",
                stock: 10,
                Guid.NewGuid()
            );
            var productReadRepository = new Mock<IProductReadRepository>();
            productReadRepository
                .Setup(r => r.GetByIdAsync(product.Id))
                .ReturnsAsync(product);
            var productWriteRepository = new Mock<IProductWriteRepository>();
            var inventoryMovementWriteRepository = new Mock<IInventoryMovementWriteRepository>();
            var validator = new Mock<IValidator<RegisterInventoryMovementCommand>>();
            var handler = new RegisterInventoryMovementCommandHandler(
                productReadRepository.Object,
                productWriteRepository.Object,
                inventoryMovementWriteRepository.Object,
                validator.Object
            );
            var command = new RegisterInventoryMovementCommand
            {
                ProductId = product.Id,
                Quantity = 4,
                Type = InventoryMovementType.Exit
            };

            await handler.HandleAsync(command);

            Assert.Equal(6, product.Stock);

            inventoryMovementWriteRepository.Verify(
                r => r.AddAsync(It.IsAny<InventoryMovement>()),
                Times.Once
            );
            productWriteRepository.Verify(
                r => r.UpdateStockAsync(product),
                Times.Once
            );
        }
    }
}