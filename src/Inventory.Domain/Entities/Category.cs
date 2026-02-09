namespace Inventory.Domain.Entities;

public sealed class Category
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public bool Status { get; private set; }

    private Category()
    {
    }

    public Category(Guid id, string name, bool status)
    {
        Id = id;
        Name = name;
        Status = status;
    }
}