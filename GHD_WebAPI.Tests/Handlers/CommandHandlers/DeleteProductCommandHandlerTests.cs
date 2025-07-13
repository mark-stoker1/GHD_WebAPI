using GHD_WebAPI.Data.DataEntities;
using GHD_WebAPI.Data.Interfaces;
using GHD_WebAPI.Handlers.CommandHandlers;
using GHD_WebAPI.Handlers.CommandHandlers.Commands;
using GHD_WebAPI.Tests.TestHelpers;
using Microsoft.Extensions.Logging;
using Moq;

namespace GHD_WebAPI.Tests.Handlers.CommandHandlers
{
    public class DeleteProductCommandHandlerTests
    {
        private readonly Mock<IProductsRepository> _productsRepository;
        private readonly Mock<ILogger<DeleteProductCommandHandler>> _logger;
        private readonly DeleteProductCommandHandler _deleteProductCommandHandler;

        public DeleteProductCommandHandlerTests()
        {
            _productsRepository = new Mock<IProductsRepository>();
            _logger = new Mock<ILogger<DeleteProductCommandHandler>>();
            _deleteProductCommandHandler = new DeleteProductCommandHandler(_productsRepository.Object, _logger.Object);
        }

        [Fact]
        public async Task DeleteProductCommandHandler_ValidCommandObjectPassedAndProductExists_ProductIsMarkedForDeletion()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "some product",
                Brand = "OlssonFireAndRisk",
                Price = 100.00m,
                IsDeleted = false
            };

            _productsRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            var command = new DeleteProductCommand { Id = 1 };
            var cancellationToken = CancellationToken.None;

            Product? capturedProduct = null;

            _productsRepository
                .Setup(repo => repo.DeleteAsync(It.IsAny<Product>(), cancellationToken))
                .Callback<Product, CancellationToken>((p, _) => capturedProduct = p)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _deleteProductCommandHandler.Handle(command, cancellationToken);

            // Assert
            Assert.True(result);
            Assert.NotNull(capturedProduct);
            Assert.True(capturedProduct!.IsDeleted);
            _productsRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task DeleteProductCommandHandler_ValidCommandObjectPassedAndProductDoesNotExists_ProductIsMarkedForDeletion()
        {
            // Arrange
            Product? product = null;

            _productsRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            var command = new DeleteProductCommand { Id = 20 };
            var cancellationToken = CancellationToken.None;

            var expectedLoggedWarning = "Delete failed: Product with ID 20 not found.";

            // Act
            var result = await _deleteProductCommandHandler.Handle(command, cancellationToken);

            // Assert
            Assert.False(result);
            _logger.VerifyLogWarning(expectedLoggedWarning, Times.Once());
            _productsRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task DeleteProductCommandHandler_NullCommandObjectPassed_ArgumentNullExceptionThrownAndLogged()
        {
            // Arrange
            DeleteProductCommand? command = null;
            var cancellationToken = CancellationToken.None;

            var expectedExceptionMessage = "Value cannot be null. (Parameter 'command')";

            // Act and Assert
            var result = await Assert.ThrowsAsync<ArgumentNullException>(async () => await _deleteProductCommandHandler.Handle(command!, cancellationToken));
            Assert.Equal(expectedExceptionMessage, result.Message);
            _logger.VerifyLogError<DeleteProductCommandHandler, ArgumentNullException>(expectedExceptionMessage, Times.Once());
            _productsRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never());
        }
    }
}
