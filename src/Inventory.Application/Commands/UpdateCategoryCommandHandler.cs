using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;

namespace Inventory.Application.Commands
{
    public sealed class UpdateCategoryCommandHandler
    {
        private readonly ICategoryWriteRepository categoryWriteRepository;

        public UpdateCategoryCommandHandler(ICategoryWriteRepository categoryWriteRepository)
        {
            this.categoryWriteRepository = categoryWriteRepository;
        }

        public async Task HandleAsync(UpdateCategoryCommand command)
        {
            var category = new Category(
                command.Id,
                command.Name,
                true);

            await categoryWriteRepository.UpdateAsync(category);
        }
    }
}