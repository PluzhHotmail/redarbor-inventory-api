namespace Inventory.Domain.Entities;

public sealed class Product
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public Guid CategoryId { get; private set; }

    public int Stock { get; private set; }

    private Product()
    {
    }

    public Product(Guid id, string name, Guid categoryId)
    {
        Id = id;
        Name = name;
        CategoryId = categoryId;
        Stock = 0;
    }

    public void IncreaseStock(int quantity)
    {
        Stock += quantity;
    }

    public void DecreaseStock(int quantity)
    {
        if (quantity > Stock)
        {
            throw new InvalidOperationException("Insufficient stock");
        }

        Stock -= quantity;
    }
}