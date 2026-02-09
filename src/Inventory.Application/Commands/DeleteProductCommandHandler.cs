using Inventory.Application.Interfaces;

namespace Inventory.Application.Commands
{
    public sealed class DeleteProductCommandHandler
    {
        private readonly IProductWriteRepository productWriteRepository;

        public DeleteProductCommandHandler(IProductWriteRepository productWriteRepository)
        {
            this.productWriteRepository = productWriteRepository;
        }
        public async Task HandleAsync(DeleteProductCommand command)
        {
            await productWriteRepository.DeleteAsync(command.Id);
        }
    }
}