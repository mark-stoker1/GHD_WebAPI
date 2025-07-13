using FluentValidation;
using GHD_WebAPI.Handlers.QueryHandlers.Queries;

namespace GHD_WebAPI.Validators
{
    /// <summary>
    /// Validator for ProductQuery.
    /// </summary>
    public class ProductQueryValidator : AbstractValidator<ProductQuery>
    {
        /// <summary>
        /// Defines validation rules for ProductQuery.
        /// </summary>
        public ProductQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Product ID must be greater than zero.");
        }
    }
}
