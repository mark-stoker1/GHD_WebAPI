using FluentAssertions;
using GHD_WebAPI.Data.DataEntities;
using GHD_WebAPI.Data.Interfaces;
using GHD_WebAPI.Enums;
using GHD_WebAPI.Handlers.CommandHandlers;
using GHD_WebAPI.Handlers.CommandHandlers.Commands;
using GHD_WebAPI.Handlers.DTOs;
using GHD_WebAPI.Tests.TestHelpers;
using Microsoft.Extensions.Logging;
using Moq;

namespace GHD_WebAPI.Tests.Handlers.CommandHandlers
{
    public class UpdateProductCommandHandlerTests
    {
        private readonly Mock<IProductsRepository> _productsRepository;
        private readonly Mock<ILogger<UpdateProductCommandHandler>> _logger;
        private readonly UpdateProductCommandHandler _updateProductCommandHandler;

        public UpdateProductCommandHandlerTests()
        {
            _productsRepository = new Mock<IProductsRepository>();
            _logger = new Mock<ILogger<UpdateProductCommandHandler>>();
            _updateProductCommandHandler = new UpdateProductCommandHandler(_productsRepository.Object, _logger.Object);
        }

        [Fact]
        public async Task UpdateProductCommandHandler_ProductExistsAndNoConflictWithAnotherNameAndBrand_ProductIsCreated()
        {
            // Arrange
            var product = new Product
            {
                Id = 40,
                Name = "Transportation",
                Brand = "GHDDigital",
                Price = 999.00m,
                IsDeleted = false
            };

            _productsRepository.Setup(_productsRepository => _productsRepository.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            _productsRepository.Setup(repo => repo.ProductExistsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Updated price
            var command = new UpdateProductCommand { Id = 40, Name = "Transportation", Brand = Brand.GHDDigital, Price = 15000.00m };

            var cancellationToken = CancellationToken.None;

            var expectedResult = (
                success: true,
                error: (string?)null,
                product: new ProductDto { Id = 40, Name = command.Name, Brand = command.Brand, Price = command.Price });

            // Act
            var result = await _updateProductCommandHandler.Handle(command, cancellationToken);

            // Assert
            Assert.True(result.success);
            Assert.Null(result.error);
            result.product.Should().BeEquivalentTo(expectedResult.product);
            _productsRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once());
            _productsRepository.Verify(repo => repo.ProductExistsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task UpdateProductCommandHandler_ProductDoesNotExist_ReturnsErrorLogsWarning()
        {
            // Arrange
            Product? product = null;

            _productsRepository.Setup(_productsRepository => _productsRepository.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);


            // Updated price
            var command = new UpdateProductCommand { Id = 99, Name = "Transportation", Brand = Brand.GHDDigital, Price = 15000.00m };

            var cancellationToken = CancellationToken.None;

            var expectedResult = (
                success: false,
                error: "Product with ID 99 not found.",
                product: (ProductDto?)null);

            var expectedLoggedWarning = "Update failed: Product with ID 99 not found.";

            // Act
            var result = await _updateProductCommandHandler.Handle(command, cancellationToken);

            // Assert
            Assert.False(result.success);
            Assert.Equal(expectedResult.error, result.error);
            _logger.VerifyLogWarning(expectedLoggedWarning, Times.Once());
            _productsRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once());
            _productsRepository.Verify(repo => repo.ProductExistsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Fact]
        public async Task UpdateProductCommandHandler_CombinationOfNameAndBrandAlreadyExists_ReturnsErrorLogsWarning()
        {
            // Arrange
            var product = new Product
            {
                Id = 40,
                Name = "Transportation",
                Brand = "GHDDigital",
                Price = 999.00m,
                IsDeleted = false
            };

            _productsRepository.Setup(_productsRepository => _productsRepository.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            _productsRepository.Setup(repo => repo.ProductExistsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Updated price
            var command = new UpdateProductCommand { Id = 40, Name = "Transportation", Brand = Brand.GHDDigital, Price = 15000.00m };

            var cancellationToken = CancellationToken.None;
            var expectedResult = (
                success: false,
                error: "Product 'Transportation' with brand 'GHDDigital' already exists.",
                product: (ProductDto?)null);

            var expectedLoggedWarning = "Update failed: Product 'Transportation' with brand 'GHDDigital' already exists.";

            // Act
            var result = await _updateProductCommandHandler.Handle(command, cancellationToken);

            // Assert
            Assert.False(result.success);
            Assert.Equal(expectedResult.error, result.error);
            _logger.VerifyLogWarning(expectedLoggedWarning, Times.Once());
            _productsRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once());
            _productsRepository.Verify(repo => repo.ProductExistsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task UpdateProductCommandHandler_CommandObjectIsNull_ThrowsAndLogsArgumentNullException()
        {
            // Arrange
            UpdateProductCommand? command = null;

            var cancellationToken = CancellationToken.None;

            var expectedExceptionMessage = "Value cannot be null. (Parameter 'command')";

            // Act and assert
            var result = await Assert.ThrowsAsync<ArgumentNullException>(async () => await _updateProductCommandHandler.Handle(command!, cancellationToken));
            Assert.Equal(expectedExceptionMessage, result.Message);
            _logger.VerifyLogError<UpdateProductCommandHandler, ArgumentNullException>(expectedExceptionMessage, Times.Once());
            _productsRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never());
            _productsRepository.Verify(repo => repo.ProductExistsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never());
        }
    }
}
