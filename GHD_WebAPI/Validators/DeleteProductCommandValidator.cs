using FluentValidation;
using GHD_WebAPI.Handlers.CommandHandlers.Commands;

namespace GHD_WebAPI.Validators
{
    /// <summary>
    /// Validator for the DeleteProductCommand.
    /// </summary>
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        /// <summary>
        /// Defines validation rules for DeleteProductCommand.
        /// </summary>
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Product ID must be greater than zero.");
        }
    }
}
