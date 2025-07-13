using GHD_WebAPI.Enums;
using GHD_WebAPI.Handlers.CommandHandlers.Commands;
using GHD_WebAPI.Validators;

namespace GHD_WebAPI.Tests.Validators
{
    public class UpdateProductCommandValidatorTests
    {
        private readonly UpdateProductCommandValidator _validator = new();

        [Fact]
        public void UpdateProductCommandValidator_ValidCommandObject_IsValid()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                Id = 1,
                Name = "Test Product",
                Brand = Brand.GHDDigital,
                Price = 99.999m
            };

            var expectedErrorCount = 0;

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
            Assert.Equal(expectedErrorCount, result.Errors.Count);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void UpdateProductCommandValidator_InvalidProductId_Invalid(int invalidId)
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                Id = invalidId,
                Name = "Test Product",
                Brand = Brand.GHDDigital,
                Price = 99.999m
            };

            var expectedErrorCount = 1;
            var expectedErrorMessage = "Id must be greater than zero.";

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(expectedErrorCount, result.Errors.Count);
            Assert.Equal(expectedErrorMessage, result.Errors[0].ErrorMessage);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void UpdateProductCommandValidator_NameNullOrEmpty_Invalid(string invalidName)
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                Id = 10,
                Name = invalidName,
                Brand = Brand.GHDDigital,
                Price = 99.999m
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
        public void UpdateProductCommandValidator_NameIsGreaterThanOneHundredCharacters_Invalid()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                Id = 10,
                Name = new string('b', 101),
                Brand = Brand.GHDDigital,
                Price = 99.999m
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
        public void UpdateProductCommandValidator_InvalidBrandName_Invalid()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                Id = 10,
                Name = "some product name",
                Brand = (Brand)(-1),
                Price = 99.999m
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
        public void UpdateProductCommandValidator_PriceLessThanZero_Invalid()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                Id = 10,
                Name = "some product name",
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
        public void UpdateProductCommandValidator_PriceExceedsMaxValue_Invalid()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                Id = 10,
                Name = "some product name",
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
