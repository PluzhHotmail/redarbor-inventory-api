using FluentValidation;
using Inventory.Application.Commands;

namespace Inventory.Application.Validators
{
    public sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}