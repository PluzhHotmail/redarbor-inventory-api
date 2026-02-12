using Inventory.Application.Interfaces;
using Inventory.Application.Queries;
using Inventory.Domain.Entities;
using Moq;

namespace Inventory.Tests.Queries
{
    public class GetAllProductsQueryHandlerTests
    {
        private readonly Mock<IProductReadRepository> _readRepositoryMock;
        private readonly GetProductsQueryHandler _handler;

        public GetAllProductsQueryHandlerTests()
        {
            _readRepositoryMock = new Mock<IProductReadRepository>();
            _handler = new GetProductsQueryHandler(_readRepositoryMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_All_Products()
        {
            var products = new List<Product>
            {
                new Product(Guid.NewGuid(), "P1", 5, Guid.NewGuid()),
                new Product(Guid.NewGuid(), "P2", 10, Guid.NewGuid())
            };
            _readRepositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(products);
            var query = new GetProductsQuery();
            var result = await _handler.HandleAsync(query);
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}