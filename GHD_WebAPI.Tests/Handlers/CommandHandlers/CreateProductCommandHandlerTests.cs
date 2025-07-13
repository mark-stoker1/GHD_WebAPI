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
    public class CreateProductCommandHandlerTests
    {
        private readonly Mock<IProductsRepository> _productsRepository;
        private readonly Mock<ILogger<CreateProductCommandHandler>> _logger;
        private readonly CreateProductCommandHandler _createProductCommandHandler;

        public CreateProductCommandHandlerTests()
        {
            _productsRepository = new Mock<IProductsRepository>();
            _logger = new Mock<ILogger<CreateProductCommandHandler>>();
            _createProductCommandHandler = new CreateProductCommandHandler(_productsRepository.Object, _logger.Object);
        }

        [Fact]
        public async Task CreateProductQueryHandler_PassValidCommandAndProductDoesNotExist_ProductIsCreatedAndSuccessReturned()
        {
            // Arrange
            _productsRepository.Setup(repo => repo.ProductExistsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

            _productsRepository.Setup(repo => repo.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                .Callback<Product, CancellationToken>((product, cancellationToken) =>
                {
                    product.Id = 50; // Mocks auto gnerated ID from database.
                });

            var createProductCommand = new CreateProductCommand
            {
                Name = "Some new Product",
                Brand = Enums.Brand.OlssonFireAndRisk,
                Price = 1000.00m
            };

            var cancellationToken = CancellationToken.None;

            var expectedResult = (true, new ProductDto
            {
                Id = 50,
                Name = createProductCommand.Name,
                Brand = createProductCommand.Brand,
                Price = createProductCommand.Price
            });

            // Act
            var result = await _createProductCommandHandler.Handle(createProductCommand, cancellationToken);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
            _productsRepository.Verify(repo => repo.ProductExistsAsync(It.IsAny<string>(), It.IsAny<string>(), cancellationToken), Times.Once());
            _productsRepository.Verify(repo => repo.AddAsync(It.IsAny<Product>(), cancellationToken), Times.Once());
        }

        [Fact]
        public async Task CreateProductQueryHandler_PassValidCommandAndProductAlreadyExists_WarningIsLoggedAndFailureReturned()
        {
            // Arrange
            _productsRepository.Setup(repo => repo.ProductExistsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var createProductCommand = new CreateProductCommand
            {
                Name = "Some new Product",
                Brand = Brand.OlssonFireAndRisk,
                Price = 1000.00m
            };

            var cancellationToken = CancellationToken.None;

            (bool Success, ProductDto? Product) expectedResult = (false, null);
            var expectedLoggedWarning = "Create failed: Product 'Some new Product' with brand 'OlssonFireAndRisk' already exists.";

            // Act
            var result = await _createProductCommandHandler.Handle(createProductCommand, cancellationToken);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
            _logger.VerifyLogWarning(expectedLoggedWarning, Times.Once());
            _productsRepository.Verify(repo => repo.ProductExistsAsync(It.IsAny<string>(), It.IsAny<string>(), cancellationToken), Times.Once());
            _productsRepository.Verify(repo => repo.AddAsync(It.IsAny<Product>(), cancellationToken), Times.Never());
        }

        [Fact]
        public async Task CreateProductQueryHandler_NullCommandObjectPassedAsParam_WarningIsLoggedAndFailureReturned()
        {
            // Arrange
            CreateProductCommand? createProductCommand = null;
            var cancellationToken = CancellationToken.None;

            var expectedExceptionMessage = "Value cannot be null. (Parameter 'command')";

            // Act and Assert
            var result = await Assert.ThrowsAsync<ArgumentNullException>(async () => await _createProductCommandHandler.Handle(createProductCommand!, cancellationToken));
            Assert.Equal(expectedExceptionMessage, result.Message);
            _logger.VerifyLogError<CreateProductCommandHandler, ArgumentNullException>(expectedExceptionMessage, Times.Once());
            _productsRepository.Verify(repo => repo.ProductExistsAsync(It.IsAny<string>(), It.IsAny<string>(), cancellationToken), Times.Never());
            _productsRepository.Verify(repo => repo.AddAsync(It.IsAny<Product>(), cancellationToken), Times.Never());
        }
    }
}
