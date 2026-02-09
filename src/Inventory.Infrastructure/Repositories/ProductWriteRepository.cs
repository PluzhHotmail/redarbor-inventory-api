using System.Data;
using Dapper;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;

namespace Inventory.Infrastructure.Repositories;

public sealed class ProductWriteRepository : IProductWriteRepository
{
    private readonly IDbConnection connection;

    public ProductWriteRepository(IDbConnection connection)
    {
        this.connection = connection;
    }

    public async Task CreateAsync(Product product)
    {
        const string sql = @"
        INSERT INTO Products (Id, Name, Stock, Status, CategoryId)
        VALUES (@Id, @Name, @Stock, @Status, @CategoryId)";

        await connection.ExecuteAsync(sql, product);
    }

    public async Task UpdateAsync(Product product)
    {
        const string sql = @"
        UPDATE Products
        SET Name = @Name,
            Stock = @Stock,
            CategoryId = @CategoryId
        WHERE Id = @Id";

        await connection.ExecuteAsync(sql, product);
    }

    public async Task DeleteAsync(Guid id)
    {
        const string sql = @"
        UPDATE Products
        SET Status = @Status
        WHERE Id = @Id";

        await connection.ExecuteAsync(sql, new
        {
            Id = id,
            Status = false
        });
    }
}