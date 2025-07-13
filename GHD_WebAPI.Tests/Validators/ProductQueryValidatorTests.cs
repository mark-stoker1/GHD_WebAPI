using FluentValidation.TestHelper;
using GHD_WebAPI.Handlers.QueryHandlers.Queries;
using GHD_WebAPI.Validators;

namespace GHD_WebAPI.Tests.Validators
{
    public class ProductQueryValidatorTests
    {
        private readonly ProductQueryValidator _validator = new();

        [Fact]
        public void ProductQueryValidator_ValidIdPassed_IsValid()
        {
            // Arrange
            var model = new ProductQuery { Id = 123 };

            var expectedErrorCount = 0;

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            Assert.True(result.IsValid);
            Assert.Equal(expectedErrorCount, result.Errors.Count);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-20)]
        public void ProductQueryValidator_IdIsLessThanOrEqualToZero_Invalid(int invalidId)
        {
            // Arrange
            var model = new ProductQuery { Id = invalidId };

            var expectedErrorCount = 1;
            var expectedErrorMessage = "Product ID must be greater than zero.";

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(expectedErrorCount, result.Errors.Count);
            Assert.Equal(expectedErrorMessage, result.Errors[0].ErrorMessage);
        }
    }
}
