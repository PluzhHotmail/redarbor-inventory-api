using FluentValidation;
using Inventory.Application.Exceptions;
using Inventory.Application.Interfaces;

namespace Inventory.Application.Commands
{
    public sealed class UpdateProductCommandHandler
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IValidator<UpdateProductCommand> _validator;

        public UpdateProductCommandHandler(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, IValidator<UpdateProductCommand> validator)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _validator = validator;
        }

        public async Task HandleAsync(UpdateProductCommand command, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var existingProduct = await _productReadRepository.GetByIdAsync(command.Id);
            if (existingProduct is null)
            {
                throw new NotFoundException("Product not found.");
            }
            existingProduct.Update(command.Name, command.Stock, command.CategoryId);
            await _productWriteRepository.UpdateAsync(existingProduct);
        }
    }
}