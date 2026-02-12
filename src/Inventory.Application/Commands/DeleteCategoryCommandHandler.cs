using FluentValidation;
using Inventory.Application.Interfaces;

namespace Inventory.Application.Commands
{
    public sealed class DeleteCategoryCommandHandler
    {
        private readonly ICategoryWriteRepository categoryWriteRepository;
        private readonly IValidator<DeleteCategoryCommand> validator;

        public DeleteCategoryCommandHandler(ICategoryWriteRepository categoryWriteRepository, IValidator<DeleteCategoryCommand> validator)
        {
            this.categoryWriteRepository = categoryWriteRepository;
            this.validator = validator;
        }
        public async Task HandleAsync(DeleteCategoryCommand command)
        {
            await validator.ValidateAndThrowAsync(command);
            await categoryWriteRepository.DeleteAsync(command.Id);
        }
    }
}