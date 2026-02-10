using Inventory.Domain.Entities;

namespace Inventory.Application.Interfaces
{
    public interface IInventoryMovementWriteRepository
    {
        Task AddAsync(InventoryMovement movement);
    }
}