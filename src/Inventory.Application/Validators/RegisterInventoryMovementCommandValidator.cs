using FluentValidation;
using Inventory.Application.Commands;

namespace Inventory.Application.Validators
{
    public sealed class RegisterInventoryMovementCommandValidator : AbstractValidator<RegisterInventoryMovementCommand>
    {
        public RegisterInventoryMovementCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty();
            RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage("Invalid movement type.");
        }
    }
}