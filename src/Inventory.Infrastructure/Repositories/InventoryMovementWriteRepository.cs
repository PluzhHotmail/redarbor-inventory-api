using Dapper;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;
using System.Data;

namespace Inventory.Infrastructure.Repositories
{
    public sealed class InventoryMovementWriteRepository : IInventoryMovementWriteRepository
    {
        private readonly IDbConnection connection;

        public InventoryMovementWriteRepository(IDbConnection connection)
        {
            this.connection = connection;
        }
        public async Task AddAsync(InventoryMovement movement)
        {
            const string sql = @"
            INSERT INTO InventoryMovements (Id, ProductId, Quantity, Type, CreatedAt)
            VALUES (@Id, @ProductId, @Quantity, @Type, @CreatedAt)";

            await connection.ExecuteAsync(sql, new
            {
                movement.Id,
                movement.ProductId,
                movement.Quantity,
                Type = (int)movement.Type,
                movement.CreatedAt
            });
        }
    }
}