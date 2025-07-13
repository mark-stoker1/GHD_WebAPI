using FluentValidation;
using GHD_WebAPI.Handlers.QueryHandlers.Queries;

namespace GHD_WebAPI.Validators
{
    /// <summary>
    /// Validator for ProductsQuery.
    /// </summary>
    public class ProductsQueryValidator : AbstractValidator<ProductsQuery>
    {
        /// <summary>
        /// Defines validation rules for ProductsQuery.
        /// </summary>
        public ProductsQueryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0)
                .WithMessage("Page must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("PageSize must be between 1 and 100.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name cannot be white space characters.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.")
                .When(x => x.Name is not null);

            RuleFor(x => x.Brand!.Value)
                .IsInEnum().WithMessage("Not a valid brand name.")
                .When(x => x.Brand.HasValue);
        }
    }
}
