using GHD_WebAPI.Handlers.CommandHandlers.Commands;
using GHD_WebAPI.Validators;

namespace GHD_WebAPI.Tests.Validators
{
    public class DeleteProductCommandValidatorTests
    {
        private readonly DeleteProductCommandValidator _validator = new();

        [Fact]
        public void DeleteProductCommandValidator_ValidCommandObject_IsValid()
        {
            // Arrange
            var command = new DeleteProductCommand
            {
                Id = 1
            };

            var expectedErrorCount = 0;

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
            Assert.Equal(expectedErrorCount, result.Errors.Count);
        }

        [Fact]
        public void DeleteProductCommandValidator_IdLessThanZero_IsValid()
        {
            // Arrange
            var command = new DeleteProductCommand
            {
                Id = -10
            };

            var expectedErrorCount = 1;
            var expectedErrorMessage = "Product ID must be greater than zero.";

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(expectedErrorCount, result.Errors.Count);
            Assert.Equal(result.Errors[0].ErrorMessage, expectedErrorMessage);
        }
    }
}
