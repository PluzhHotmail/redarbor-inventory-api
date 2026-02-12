using FluentValidation;
using FluentValidation.Results;
using Inventory.Application.Commands;
using Inventory.Application.Exceptions;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;
using Moq;

namespace Inventory.Tests.Commands
{
    public class CreateProductCommandHandlerTests
    {
        private readonly Mock<IValidator<CreateProductCommand>> _validatorMock;
        private readonly Mock<IProductWriteRepository> _writeRepositoryMock;
        private readonly Mock<ICategoryReadRepository> _categoryReadRepositoryMock;
        private readonly CreateProductCommandHandler _handler;

        public CreateProductCommandHandlerTests()
        {
            _validatorMock = new Mock<IValidator<CreateProductCommand>>();
            _writeRepositoryMock = new Mock<IProductWriteRepository>();
            _categoryReadRepositoryMock = new Mock<ICategoryReadRepository>();

            _handler = new CreateProductCommandHandler(_writeRepositoryMock.Object, _categoryReadRepositoryMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Create_Product_When_Command_Is_Valid()
        {
            var categoryId = Guid.NewGuid();
            var command = new CreateProductCommand
            {
                Name = "Product Test",
                Stock = 10,
                CategoryId = categoryId
            };
            _validatorMock
                .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _categoryReadRepositoryMock
                .Setup(r => r.GetByIdAsync(categoryId))
                .ReturnsAsync(new Category(categoryId, "Category Test", true));
            await _handler.HandleAsync(command);
            _writeRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_Command_Is_Invalid()
        {
            var command = new CreateProductCommand();
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("Name", "Required")
            };
            _validatorMock
                .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(failures));
            await Assert.ThrowsAsync<ValidationException>(() => _handler.HandleAsync(command));
            _writeRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_NotFoundException_When_Category_Does_Not_Exist()
        {
            var categoryId = Guid.NewGuid();
            var command = new CreateProductCommand
            {
                Name = "Product Test",
                Stock = 10,
                CategoryId = categoryId
            };
            _validatorMock
                .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _categoryReadRepositoryMock
                .Setup(r => r.GetByIdAsync(categoryId))
                .ReturnsAsync((Category?)null);
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.HandleAsync(command));
            _writeRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Product>()), Times.Never);
        }
    }
}