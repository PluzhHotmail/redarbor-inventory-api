using Inventory.Domain.Entities;

namespace Inventory.Application.Interfaces
{
    public interface ICategoryReadRepository
    {
        Task<Category?> GetByIdAsync(Guid id);

        Task<IReadOnlyCollection<Category>> GetAllAsync();
    }
}