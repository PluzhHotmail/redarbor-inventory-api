namespace Inventory.Domain.Entities;

public sealed class Product
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public int Stock { get; set; } = 0;

    public bool Status { get; private set; }

    public Guid CategoryId { get; private set; }

    private Product()
    {
    }

    public Product(Guid id, string name, int stock, Guid categoryId)
    {
        Id = id;
        Name = name;
        Stock = stock;
        Status = true;
        CategoryId = categoryId;
    }

    public void Update(string name, int stock, Guid categoryId)
    {
        Name = name;
        Stock = stock;
        CategoryId = categoryId;
    }

}