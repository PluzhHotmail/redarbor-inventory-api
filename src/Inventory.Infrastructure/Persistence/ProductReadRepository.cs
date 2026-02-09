using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories
{
    public sealed class ProductReadRepository : IProductReadRepository
    {
        private readonly InventoryReadDbContext context;

        public ProductReadRepository(InventoryReadDbContext context)
        {
            this.context = context;
        }

        public async Task<IReadOnlyCollection<Product>> GetAllAsync()
        {
            return await context.Products.Where(p => p.Status == true).AsNoTracking().ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await context.Products.AsNoTracking()
                .FirstOrDefaultAsync(product => product.Id == id);
        }
    }
}