using FluentValidation.TestHelper;
using GHD_WebAPI.Enums;
using GHD_WebAPI.Handlers.QueryHandlers.Queries;
using GHD_WebAPI.Validators;

namespace GHD_WebAPI.Tests.Validators
{
    public class ProductsQueryValidatorTests
    {
        private readonly ProductsQueryValidator _validator = new();

        [Theory]
        [InlineData("Advisory Services", Brand.GHDWoodhead)]
        [InlineData(null, null)]
        public void ProductsQueryValidator_QueryIsValid_IsValid(string name, Brand? brand)
        {
            // Arrange
            var query = new ProductsQuery
            {
                Page = 1,
                PageSize = 10,
                Name = name,
                Brand = brand
            };

            var expectedErrorCount = 0;

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            Assert.True(result.IsValid);
            Assert.Equal(expectedErrorCount, result.Errors.Count);
        }

        [Theory]
        [InlineData(-10)]
        [InlineData(0)]
        public void ProductsQueryValidator_PageIsLessThan1_Invalid(int invalidPage)
        {
            // Arrange
            var query = new ProductsQuery { Page = invalidPage, PageSize = 10 };

            var expectedErrorCount = 1;
            var expectedValidationError = "Page must be greater than 0.";

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(expectedValidationError, result.Errors[0].ErrorMessage);
            Assert.Equal(expectedErrorCount, result.Errors.Count);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(101)]
        public void ProductsQueryValidator_PageSizeMustBeBetween1and100_Invalid(int invalidPageSize)
        {
            // Arrange
            var query = new ProductsQuery { Page = 1, PageSize = invalidPageSize };

            var expectedErrorCount = 1;
            var expectedValidationError = "PageSize must be between 1 and 100.";

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(expectedValidationError, result.Errors[0].ErrorMessage);
            Assert.Equal(expectedErrorCount, result.Errors.Count);
        }

        [Fact]
        public void ProductsQueryValidator_NameIsWhiteSpace_Invalid()
        {
            // Arrange
            var query = new ProductsQuery
            {
                Page = 1,
                PageSize = 10,
                Name = " ",
                Brand = Brand.GHDWoodhead
            };

            var expectedErrorCount = 1;
            var expectedValidationError = "Name cannot be white space characters.";

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(expectedValidationError, result.Errors[0].ErrorMessage);
            Assert.Equal(expectedErrorCount, result.Errors.Count);
        }

        [Fact]
        public void ProductsQueryValidator_NameIsGreaterThan100Characters_Invalid()
        {
            // Arrange
            var query = new ProductsQuery
            {
                Page = 1,
                PageSize = 10,
                Name = new string('x', 101),
                Brand = Brand.GHDWoodhead
            };

            var expectedErrorCount = 1;
            var expectedValidationError = "Name must not exceed 100 characters.";

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(expectedValidationError, result.Errors[0].ErrorMessage);
            Assert.Equal(expectedErrorCount, result.Errors.Count);
        }

        [Fact]
        public void ProductsQueryValidator_InvalidBrandName_Invalid()
        {
            // Arrange
            var query = new ProductsQuery
            {
                Page = 1,
                PageSize = 10,
                Name = "Product B",
                Brand = (Brand)(-1)
            };

            var expectedErrorCount = 1;
            var expectedErrorMessage = "Not a valid brand name.";

            // Act
            var result = _validator.Validate(query);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(expectedErrorCount, result.Errors.Count);
            Assert.Equal(expectedErrorMessage, result.Errors[0].ErrorMessage);
        }
    }
}
