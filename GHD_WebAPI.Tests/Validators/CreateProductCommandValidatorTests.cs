using GHD_WebAPI.Enums;
using GHD_WebAPI.Handlers.CommandHandlers.Commands;
using GHD_WebAPI.Validators;

namespace GHD_WebAPI.Tests.Validators
{
    public class CreateProductCommandValidatorTests
    {
        private readonly CreateProductCommandValidator _validator = new();

        [Fact]
        public void CreateProductCommandValidator_ValidCommandObject_IsValid()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "Test Product",
                Brand = Brand.MovementStrategies,
                Price = 10.99m
            };

            var expectedErrorCount = 0;

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
            Assert.Equal(expectedErrorCount, result.Errors.Count);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void CreateProductCommandValidator_NameIsEmpty_Invalid(string invalidName)
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = invalidName,
                Brand = Brand.MovementStrategies,
                Price = 10.99m
            };


            var expectedErrorCount = 1;
            var expectedErrorMessage = "Name is required.";

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(expectedErrorCount, result.Errors.Count);
            Assert.Equal(expectedErrorMessage, result.Errors[0].ErrorMessage);
        }

        [Fact]
        public void CreateProductCommandValidator_NameLongerThanOneHundredCharacters_Invalid()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = new string('A', 150),
                Brand = Brand.MovementStrategies,
                Price = 10.99m
            };

            var expectedErrorCount = 1;
            var expectedErrorMessage = "Name must not exceed 100 characters.";

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(expectedErrorCount, result.Errors.Count);
            Assert.Equal(expectedErrorMessage, result.Errors[0].ErrorMessage);
        }

        [Fact]
        public void CreateProductCommandValidator_InvalidBrandName_Invalid()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "some product name",
                Brand = (Brand)(-1),
                Price = 10.99m
            };

            var expectedErrorCount = 1;
            var expectedErrorMessage = "Not a valid brand name.";

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(expectedErrorCount, result.Errors.Count);
            Assert.Equal(expectedErrorMessage, result.Errors[0].ErrorMessage);
        }

        [Fact]
        public void CreateProductCommandValidator_PriceLessThanZero_Invalid()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "Product B",
                Brand = Brand.eSolutionsGroup,
                Price = -999.99m
            };

            var expectedErrorCount = 1;
            var expectedErrorMessage = "Price must be greater than zero.";

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(expectedErrorCount, result.Errors.Count);
            Assert.Equal(expectedErrorMessage, result.Errors[0].ErrorMessage);
        }

        [Fact]
        public void CreateProductCommandValidator_PriceExceedsMaxValue_Invalid()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "Product B",
                Brand = Brand.eSolutionsGroup,
                Price = 1000000.00m
            };

            var expectedErrorCount = 1;
            var expectedErrorMessage = "Price must not exceed 999,999.99.";

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(expectedErrorCount, result.Errors.Count);
            Assert.Equal(expectedErrorMessage, result.Errors[0].ErrorMessage);
        }
    }
}
