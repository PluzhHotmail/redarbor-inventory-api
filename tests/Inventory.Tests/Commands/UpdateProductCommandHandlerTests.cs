using FluentValidation;
using FluentValidation.Results;
using Inventory.Application.Commands;
using Inventory.Application.Exceptions;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;
using Moq;

namespace Inventory.Tests.Commands
{
    public class UpdateProductCommandHandlerTests
    {
        private readonly Mock<IValidator<UpdateProductCommand>> _validatorMock;
        private readonly Mock<IProductReadRepository> _readRepositoryMock;
        private readonly Mock<IProductWriteRepository> _writeRepositoryMock;
        private readonly UpdateProductCommandHandler _handler;

        public UpdateProductCommandHandlerTests()
        {
            _validatorMock = new Mock<IValidator<UpdateProductCommand>>();
            _readRepositoryMock = new Mock<IProductReadRepository>();
            _writeRepositoryMock = new Mock<IProductWriteRepository>();

            _handler = new UpdateProductCommandHandler(
                _readRepositoryMock.Object,
                _writeRepositoryMock.Object,
                _validatorMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Update_Product_When_Command_Is_Valid()
        {
            var command = new UpdateProductCommand
            {
                Id = Guid.NewGuid(),
                Name = "Updated",
                Stock = 20,
                CategoryId = Guid.NewGuid()
            };
            var product = new Product(command.Id, "Old Name", 5, command.CategoryId);
            _validatorMock
                .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _readRepositoryMock
                .Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync(product);
            await _handler.HandleAsync(command);
            _writeRepositoryMock.Verify(
                r => r.UpdateAsync(It.Is<Product>(p =>
                    p.Name == command.Name &&
                    p.Stock == command.Stock &&
                    p.CategoryId == command.CategoryId)), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_Command_Is_Invalid()
        {
            var command = new UpdateProductCommand();
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("Name", "Required")
            };
            _validatorMock
                .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(failures));
            await Assert.ThrowsAsync<ValidationException>(
                () => _handler.HandleAsync(command));
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_NotFoundException_When_Product_Does_Not_Exist()
        {
            var command = new UpdateProductCommand
            {
                Id = Guid.NewGuid(),
                Name = "Updated",
                Stock = 10,
                CategoryId = Guid.NewGuid()
            };
            _validatorMock
                .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _readRepositoryMock
                .Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync((Product?)null);
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.HandleAsync(command));
        }
    }
}