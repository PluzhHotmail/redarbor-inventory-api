using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;

namespace Inventory.Application.Commands
{
    public sealed class CreateCategoryCommandHandler
    {
        private readonly ICategoryWriteRepository categoryWriteRepository;

        public CreateCategoryCommandHandler(ICategoryWriteRepository categoryWriteRepository)
        {
            this.categoryWriteRepository = categoryWriteRepository;
        }

        public async Task HandleAsync(CreateCategoryCommand command)
        {
            var category = new Category(
                Guid.NewGuid(),
                command.Name,
                true);

            await categoryWriteRepository.CreateAsync(category);
        }
    }
}