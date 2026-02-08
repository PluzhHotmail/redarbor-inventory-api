namespace Inventory.Domain.Entities;

public sealed class Category
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    private Category()
    {
    }

    public Category(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}