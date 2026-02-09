using Inventory.Domain.Entities;

namespace Inventory.Application.Interfaces
{
    public interface ICategoryWriteRepository
    {
        Task CreateAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(Guid id);
    }
}