using Inventory.Application.Exceptions;
using Inventory.Application.Interfaces;
using Inventory.Application.Queries;
using Inventory.Domain.Entities;
using Moq;

namespace Inventory.Tests.Queries
{
    public class GetProductByIdQueryHandlerTests
    {
        private readonly Mock<IProductReadRepository> _readRepositoryMock;
        private readonly GetProductByIdQueryHandler _handler;

        public GetProductByIdQueryHandlerTests()
        {
            _readRepositoryMock = new Mock<IProductReadRepository>();
            _handler = new GetProductByIdQueryHandler(_readRepositoryMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_Product_When_Exists()
        {
            var id = Guid.NewGuid();
            var product = new Product(id, "Test", 5, Guid.NewGuid());
            _readRepositoryMock
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(product);
            var query = new GetProductByIdQuery(id);
            var result = await _handler.HandleAsync(query);
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_NotFoundException_When_Product_Does_Not_Exist()
        {
            var id = Guid.NewGuid();
            _readRepositoryMock
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync((Product?)null);
            var query = new GetProductByIdQuery(id);
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.HandleAsync(query));
        }
    }
}