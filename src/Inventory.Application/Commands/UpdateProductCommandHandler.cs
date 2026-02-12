using FluentValidation;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;

namespace Inventory.Application.Commands
{
    public sealed class UpdateProductCommandHandler
    {
        private readonly IProductWriteRepository productWriteRepository;
        private readonly IValidator<UpdateProductCommand> validator;

        public UpdateProductCommandHandler(IProductWriteRepository productWriteRepository, IValidator<UpdateProductCommand> validator)
        {
            this.productWriteRepository = productWriteRepository;
            this.validator = validator;
        }

        public async Task HandleAsync(UpdateProductCommand command)
        {
            await validator.ValidateAndThrowAsync(command);
            var product = new Product(
                command.Id,
                command.Name,
                command.Stock,
                command.CategoryId);
            await productWriteRepository.UpdateAsync(product);
        }
    }
}