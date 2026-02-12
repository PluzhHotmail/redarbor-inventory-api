using FluentValidation;
using Inventory.Application.Interfaces;

namespace Inventory.Application.Commands
{
    public sealed class DeleteProductCommandHandler
    {
        private readonly IProductWriteRepository productWriteRepository;
        private readonly IValidator<DeleteProductCommand> validator;

        public DeleteProductCommandHandler(IProductWriteRepository productWriteRepository, IValidator<DeleteProductCommand> validator)
        {
            this.productWriteRepository = productWriteRepository;
            this.validator = validator;
        }
        public async Task HandleAsync(DeleteProductCommand command)
        {
            await validator.ValidateAndThrowAsync(command);
            await productWriteRepository.DeleteAsync(command.Id);
        }
    }
}