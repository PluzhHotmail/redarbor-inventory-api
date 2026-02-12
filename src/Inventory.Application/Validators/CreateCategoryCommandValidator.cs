using FluentValidation;
using Inventory.Application.Commands;

namespace Inventory.Application.Validators
{
    public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}