using FluentValidation;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;

namespace Inventory.Application.Commands
{
    public sealed class UpdateCategoryCommandHandler
    {
        private readonly ICategoryWriteRepository categoryWriteRepository;
        private readonly IValidator<UpdateCategoryCommand> validator;

        public UpdateCategoryCommandHandler(ICategoryWriteRepository categoryWriteRepository, IValidator<UpdateCategoryCommand> validator)
        {
            this.categoryWriteRepository = categoryWriteRepository;
            this.validator = validator;
        }

        public async Task HandleAsync(UpdateCategoryCommand command)
        {
            await validator.ValidateAndThrowAsync(command);
            var category = new Category(
                command.Id,
                command.Name,
                true);
            await categoryWriteRepository.UpdateAsync(category);
        }
    }
}