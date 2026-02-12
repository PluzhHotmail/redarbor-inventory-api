using FluentValidation;
using Inventory.Application.Exceptions;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;

namespace Inventory.Application.Commands
{
    public sealed class CreateProductCommandHandler
    {
        private readonly IProductWriteRepository productWriteRepository;
        private readonly ICategoryReadRepository categoryReadRepository;
        private readonly IValidator<CreateProductCommand> validator;

        public CreateProductCommandHandler(IProductWriteRepository productWriteRepository, ICategoryReadRepository categoryReadRepository, IValidator<CreateProductCommand> validator)
        {
            this.productWriteRepository = productWriteRepository;
            this.categoryReadRepository = categoryReadRepository;
            this.validator = validator;
        }

        public async Task HandleAsync(CreateProductCommand command, CancellationToken cancellationToken = default)
        {
            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var category = await categoryReadRepository.GetByIdAsync(command.CategoryId);
            if (category is null)
            {
                throw new NotFoundException("Category not found.");
            }
            var product = new Product(
                Guid.NewGuid(),
                command.Name,
                command.Stock,
                command.CategoryId);

            await productWriteRepository.CreateAsync(product);
        }
    }
}