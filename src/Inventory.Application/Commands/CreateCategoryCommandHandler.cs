using FluentValidation;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;

namespace Inventory.Application.Commands
{
    public sealed class CreateCategoryCommandHandler
    {
        private readonly ICategoryWriteRepository categoryWriteRepository;
        private readonly IValidator<CreateCategoryCommand> validator;

        public CreateCategoryCommandHandler(ICategoryWriteRepository categoryWriteRepository, IValidator<CreateCategoryCommand> validator)
        {
            this.categoryWriteRepository = categoryWriteRepository;
            this.validator = validator;
        }

        public async Task HandleAsync(CreateCategoryCommand command)
        {
            await validator.ValidateAndThrowAsync(command);
            var category = new Category(
                Guid.NewGuid(),
                command.Name,
                true);
            await categoryWriteRepository.CreateAsync(category);
        }
    }
}