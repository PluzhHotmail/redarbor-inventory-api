using FluentValidation;
using FluentValidation.Results;
using Inventory.Application.Commands;
using Inventory.Application.Exceptions;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;
using Moq;

namespace Inventory.Tests.Commands
{
    public class DeleteProductCommandHandlerTests
    {
        private readonly Mock<IValidator<DeleteProductCommand>> _validatorMock;
        private readonly Mock<IProductReadRepository> _readRepositoryMock;
        private readonly Mock<IProductWriteRepository> _writeRepositoryMock;
        private readonly DeleteProductCommandHandler _handler;

        public DeleteProductCommandHandlerTests()
        {
            _validatorMock = new Mock<IValidator<DeleteProductCommand>>();
            _readRepositoryMock = new Mock<IProductReadRepository>();
            _writeRepositoryMock = new Mock<IProductWriteRepository>();

            _handler = new DeleteProductCommandHandler(
                _readRepositoryMock.Object,
                _writeRepositoryMock.Object,
                _validatorMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Delete_Product_When_Command_Is_Valid()
        {
            var command = new DeleteProductCommand
            {
                Id = Guid.NewGuid()
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _readRepositoryMock
                .Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync(new Product(command.Id, "Test", 5, Guid.NewGuid()));

            await _handler.HandleAsync(command);

            _writeRepositoryMock.Verify(r => r.DeleteAsync(command.Id), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_Command_Is_Invalid()
        {
            var command = new DeleteProductCommand();
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("Id", "Required")
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(failures));

            await Assert.ThrowsAsync<ValidationException>(() => _handler.HandleAsync(command));
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_NotFoundException_When_Product_Does_Not_Exist()
        {
            var command = new DeleteProductCommand
            {
                Id = Guid.NewGuid()
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