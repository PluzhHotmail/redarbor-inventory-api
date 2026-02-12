using FluentValidation;
using Inventory.Application.Commands;

namespace Inventory.Application.Validators
{
    public sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(50);
            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0);
            RuleFor(x => x.CategoryId)
                .NotEmpty();
        }
    }
}