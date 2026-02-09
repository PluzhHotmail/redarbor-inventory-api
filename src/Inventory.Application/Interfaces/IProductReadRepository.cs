using Inventory.Domain.Entities;

namespace Inventory.Application.Interfaces
{
    public interface IProductReadRepository
    {
        Task<Product?> GetByIdAsync(Guid id);

        Task<IReadOnlyCollection<Product>> GetAllAsync();
    }
}