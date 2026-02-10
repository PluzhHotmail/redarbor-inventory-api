using Dapper;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;
using System.Data;

namespace Inventory.Infrastructure.Repositories
{
    public sealed class CategoryWriteRepository : ICategoryWriteRepository
    {
        private readonly IDbConnection connection;

        public CategoryWriteRepository(IDbConnection connection)
        {
            this.connection = connection;
        }

        public async Task CreateAsync(Category category)
        {
            const string sql = @"
            INSERT INTO Categories (Id, Name, Status)
            VALUES (@Id, @Name, @Status)";

            await connection.ExecuteAsync(sql, category);
        }

        public async Task UpdateAsync(Category category)
        {
            const string sql = @"
            UPDATE Categories
            SET Name = @Name
            WHERE Id = @Id";

            await connection.ExecuteAsync(sql, category);
        }

        public async Task DeleteAsync(Guid id)
        {
            const string sql = @"
            UPDATE Categories
            SET Status = @Status
            WHERE Id = @Id";

            await connection.ExecuteAsync(sql, new
            {
                Id = id,
                Status = false
            });
        }
    }
}