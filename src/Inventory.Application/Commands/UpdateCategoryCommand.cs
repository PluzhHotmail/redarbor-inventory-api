namespace Inventory.Application.Commands
{
    public sealed class UpdateCategoryCommand
    {
        public Guid Id { get; init; } = Guid.Empty;
        public string Name { get; init; } = string.Empty;
    }
}