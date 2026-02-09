using Inventory.Application.Interfaces;

namespace Inventory.Application.Commands
{
    public sealed class DeleteCategoryCommandHandler
    {
        private readonly ICategoryWriteRepository categoryWriteRepository;

        public DeleteCategoryCommandHandler(ICategoryWriteRepository categoryWriteRepository)
        {
            this.categoryWriteRepository = categoryWriteRepository;
        }
        public async Task HandleAsync(DeleteCategoryCommand command)
        {
            await categoryWriteRepository.DeleteAsync(command.Id);
        }
    }
}