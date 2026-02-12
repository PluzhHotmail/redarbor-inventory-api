using FluentValidation;
using Inventory.Application.Commands;

namespace Inventory.Application.Validators
{
    public sealed class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
    {
        public DeleteCategoryCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }
}