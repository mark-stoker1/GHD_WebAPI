using FluentAssertions;
using GHD_WebAPI.Data.DataEntities;
using GHD_WebAPI.Data.Interfaces;
using GHD_WebAPI.Enums;
using GHD_WebAPI.Handlers.DTOs;
using GHD_WebAPI.Handlers.QueryHandlers;
using GHD_WebAPI.Handlers.QueryHandlers.Queries;
using GHD_WebAPI.Tests.TestHelpers;
using Microsoft.Extensions.Logging;
using Moq;

namespace GHD_WebAPI.Tests.Handlers.QueryHandlers
{
    public class GetProductByIdQueryHandlerTests
    {
        private readonly Mock<IProductsRepository> _productsRepository;
        private readonly Mock<ILogger<GetProductByIdQueryHandler>> _logger;
        private readonly GetProductByIdQueryHandler _getProductByIdQueryHandler;

        public GetProductByIdQueryHandlerTests()
        {
            _productsRepository = new Mock<IProductsRepository>();
            _logger = new Mock<ILogger<GetProductByIdQueryHandler>>();
            _getProductByIdQueryHandler = new GetProductByIdQueryHandler(_productsRepository.Object, _logger.Object);
        }

        [Fact]
        public async Task GetProductByIdQueryHandler_PassValidProductId_ReturnsCorrectProduct()
        {
            // Arrange
            var productDataEntity = new Product { Id = 1, Name = "Test Product", Brand = "GHDDigital", Price = 9.99m, IsDeleted = false };
            _productsRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(productDataEntity);

            var productQuery = new ProductQuery { Id = 1 };
            var cancellationToken = CancellationToken.None;

            var expectedResult = new ProductDto { Id = 1, Name = "Test Product", Brand = Brand.GHDDigital, Price = 9.99m };

            // Act
            var result = await _getProductByIdQueryHandler.Handle(productQuery, cancellationToken);

            // Assert
            _productsRepository.Verify(repo => repo.GetByIdAsync(productQuery.Id, cancellationToken), Times.Once());
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetProductByIdQueryHandler_NoProductFound_ReturnsNull()
        {
            // Arrange
            Product? productDataEntity = null;
            _productsRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(productDataEntity);

            var productQuery = new ProductQuery { Id = 99 };
            var cancellationToken = CancellationToken.None;

            ProductDto? expectedResult = null;

            // Act
            var result = await _getProductByIdQueryHandler.Handle(productQuery, cancellationToken);

            // Assert
            _productsRepository.Verify(repo => repo.GetByIdAsync(productQuery.Id, cancellationToken), Times.Once());
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetProductByIdQueryHandler_NullQueryObjectPassedAsParameter_ThrowsAndLogsArgumentNullException()
        {
            // Arrange
            ProductQuery? productQuery = null;
            var cancellationToken = CancellationToken.None;

            var expectedExceptionMessage = "Value cannot be null. (Parameter 'query')";

            // Act and Assert
            var result = await Assert.ThrowsAsync<ArgumentNullException>(async () => await _getProductByIdQueryHandler.Handle(productQuery!, cancellationToken));
            _productsRepository.Verify(repo => repo.GetAll(), Times.Never());
            Assert.Equal(expectedExceptionMessage, result.Message);
            _logger.VerifyLogError<GetProductByIdQueryHandler, ArgumentNullException>(expectedExceptionMessage, Times.Once());
        }
    }
}
