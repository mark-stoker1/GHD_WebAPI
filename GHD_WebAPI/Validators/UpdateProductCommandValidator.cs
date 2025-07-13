using FluentValidation;
using GHD_WebAPI.Handlers.CommandHandlers.Commands;

namespace GHD_WebAPI.Validators
{
    /// <summary>
    /// Validator for UpdateProductCommand.
    /// </summary>
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        /// <summary>
        /// Defines validation rules for UpdateProductCommand.
        /// </summary>
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than zero.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.Brand)
                .IsInEnum().WithMessage("Not a valid brand name.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.")
                .LessThanOrEqualTo(999999.99m).WithMessage("Price must not exceed 999,999.99.");
        }
    }
}
