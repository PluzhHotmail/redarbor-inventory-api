using Inventory.Domain.Entities;

namespace Inventory.Application.Interfaces
{
    public interface IProductWriteRepository
    {
        Task CreateAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Guid id);
    }
}