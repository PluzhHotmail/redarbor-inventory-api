using FluentValidation;
using Inventory.Application.Exceptions;
using Inventory.Application.Interfaces;

namespace Inventory.Application.Commands
{
    public sealed class DeleteProductCommandHandler
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IValidator<DeleteProductCommand> _validator;

        public DeleteProductCommandHandler(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, IValidator<DeleteProductCommand> validator)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _validator = validator;
        }

        public async Task HandleAsync(DeleteProductCommand command, CancellationToken cancellationToken = default)
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
            await _productWriteRepository.DeleteAsync(command.Id);
        }
    }
}