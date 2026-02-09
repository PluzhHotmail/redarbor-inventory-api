using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence
{
    public sealed class CategoryReadRepository : ICategoryReadRepository
    {
        private readonly InventoryReadDbContext context;

        public CategoryReadRepository(InventoryReadDbContext context)
        {
            this.context = context;
        }

        public async Task<IReadOnlyCollection<Category>> GetAllAsync()
        {
            return await context.Categories.Where(p => p.Status == true).AsNoTracking().ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            return await context.Categories.AsNoTracking()
                .FirstOrDefaultAsync(product => product.Id == id);
        }
    }
}