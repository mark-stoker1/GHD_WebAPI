using FluentAssertions;
using GHD_WebAPI.Data.DataEntities;
using GHD_WebAPI.Data.Interfaces;
using GHD_WebAPI.Enums;
using GHD_WebAPI.Handlers.DTOs;
using GHD_WebAPI.Handlers.QueryHandlers;
using GHD_WebAPI.Handlers.QueryHandlers.Queries;
using GHD_WebAPI.Tests.TestHelpers;
using Microsoft.Extensions.Logging;
using MockQueryable;
using Moq;

namespace GHD_WebAPI.Tests.Handlers.QueryHandlers
{
    public class GetAllProductsQueryHandlerTests
    {
        private readonly Mock<IProductsRepository> _productsRepository;
        private readonly Mock<ILogger<GetAllProductsQueryHandler>> _logger;
        private readonly GetAllProductsQueryHandler _getAllProductsQueryHandler;

        public GetAllProductsQueryHandlerTests()
        {
            _productsRepository = new Mock<IProductsRepository>();
            _logger = new Mock<ILogger<GetAllProductsQueryHandler>>();
            _getAllProductsQueryHandler = new GetAllProductsQueryHandler(_productsRepository.Object, _logger.Object);
        }

        [Fact]
        public async Task GetAllProductsQueryHandler_PassValidProductsQuery_ReturnsListOfProducts()
        {
            // Arrange
            _productsRepository.Setup(repo => repo.GetAll()).Returns(GetProductDataEntities().AsQueryable());

            var productsQuery = new ProductsQuery { Page = 1, PageSize = 10 };
            var cancellationToken = CancellationToken.None;

            var expectedResult = GetProductDto();

            // Act
            var result = await _getAllProductsQueryHandler.Handle(productsQuery, cancellationToken);

            // Assert
            _productsRepository.Verify(repo => repo.GetAll(), Times.Once());
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetAllProductsQueryHandler_NoProductsFound_ReturnsEmptyList()
        {
            // Arrange
            _productsRepository.Setup(repo => repo.GetAll()).Returns(Enumerable.Empty<Product>().AsQueryable().BuildMock());

            var productsQuery = new ProductsQuery { Page = 1, PageSize = 10 };
            var cancellationToken = CancellationToken.None;

            var expectedResult = new List<ProductDto>();

            // Act
            var result = await _getAllProductsQueryHandler.Handle(productsQuery, cancellationToken);

            // Assert
            _productsRepository.Verify(repo => repo.GetAll(), Times.Once());
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllProductsQueryHandler_PageSizeSetTo1_ReturnsSingleItemInList()
        {
            // Arrange
            _productsRepository.Setup(repo => repo.GetAll()).Returns(GetProductDataEntities().AsQueryable());

            var productsQuery = new ProductsQuery { Page = 1, PageSize = 1 };
            var cancellationToken = CancellationToken.None;

            var expectedResult = new List<ProductDto>
            {
                new ProductDto { Id = 1, Name = "Advisory Services", Brand = Brand.GHDWoodhead, Price = 100.00m }
            };

            // Act
            var result = await _getAllProductsQueryHandler.Handle(productsQuery, cancellationToken);

            // Assert
            _productsRepository.Verify(repo => repo.GetAll(), Times.Once());
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetAllProductsQueryHandler_FilterResultByName_ReturnsListForOnlyThatProductName()
        {
            // Arrange
            _productsRepository.Setup(repo => repo.GetAll()).Returns(GetProductDataEntities().AsQueryable());

            var productsQuery = new ProductsQuery { Page = 1, PageSize = 100, Name = "Architecture & Desig" };
            var cancellationToken = CancellationToken.None;

            var expectedResult = new List<ProductDto>
            {
                new ProductDto {  Id = 2, Name = "Architecture & Design", Brand = Brand.GHDDigital, Price = 888.00m }
            };

            // Act
            var result = await _getAllProductsQueryHandler.Handle(productsQuery, cancellationToken);

            // Assert
            _productsRepository.Verify(repo => repo.GetAll(), Times.Once());
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetAllProductsQueryHandler_FilterResultByBrand_ReturnsListForOnlyThatProductName()
        {
            // Arrange
            _productsRepository.Setup(repo => repo.GetAll()).Returns(GetProductDataEntities().AsQueryable());

            var productsQuery = new ProductsQuery { Page = 1, PageSize = 100, Brand = Brand.eSolutionsGroup };
            var cancellationToken = CancellationToken.None;

            var expectedResult = new List<ProductDto>
            {
                new ProductDto {  Id = 4, Name = "Environmental Services", Brand = Brand.eSolutionsGroup, Price = 888.00m }
            };

            // Act
            var result = await _getAllProductsQueryHandler.Handle(productsQuery, cancellationToken);

            // Assert
            _productsRepository.Verify(repo => repo.GetAll(), Times.Once());
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetAllProductsQueryHandler_FilterResultByNameAndBrand_ReturnsListForOnlyThatProductName()
        {
            // Arrange
            _productsRepository.Setup(repo => repo.GetAll()).Returns(GetProductDataEntities().AsQueryable());

            var productsQuery = new ProductsQuery { Page = 1, PageSize = 100, Name = "Engineering & Construction", Brand = Brand.GHDAdvisory };
            var cancellationToken = CancellationToken.None;

            var expectedResult = new List<ProductDto>
            {
                new ProductDto {  Id = 3, Name = "Engineering & Construction", Brand = Brand.GHDAdvisory, Price = 888.00m },
            };

            // Act
            var result = await _getAllProductsQueryHandler.Handle(productsQuery, cancellationToken);

            // Assert
            _productsRepository.Verify(repo => repo.GetAll(), Times.Once());
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetAllProductsQueryHandler_NullQueryObjectPassedAsParameter_ThrowsAndLogsArgumentNullException()
        {
            // Arrange
            ProductsQuery? productsQuery = null;
            var cancellationToken = CancellationToken.None;

            var expectedExceptionMessage = "Value cannot be null. (Parameter 'query')";

            // Act and Assert
            var result = await Assert.ThrowsAsync<ArgumentNullException>(async () => await _getAllProductsQueryHandler.Handle(productsQuery!, cancellationToken));
            _productsRepository.Verify(repo => repo.GetAll(), Times.Never());
            Assert.Equal(expectedExceptionMessage, result.Message);
            _logger.VerifyLogError<GetAllProductsQueryHandler, ArgumentNullException>(expectedExceptionMessage, Times.Once());
        }

        private IQueryable<Product> GetProductDataEntities()
        {
            return new List<Product>
            {
                new Product { Id = 1, Name = "Advisory Services", Brand = "GHDWoodhead", Price = 100.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                new Product { Id = 2, Name = "Architecture & Design", Brand = "GHDDigital", Price = 888.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                new Product { Id = 3, Name = "Engineering & Construction", Brand = "GHDAdvisory", Price = 888.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                new Product { Id = 4, Name = "Environmental Services", Brand = "eSolutionsGroup", Price = 888.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
            }.AsQueryable().BuildMock();
        }

        private IList<ProductDto> GetProductDto()
        {
            return new List<ProductDto>
            {
                new ProductDto { Id = 1, Name = "Advisory Services", Brand = Brand.GHDWoodhead, Price = 100.00m },
                new ProductDto { Id = 2, Name = "Architecture & Design", Brand = Brand.GHDDigital, Price = 888.00m },
                new ProductDto { Id = 3, Name = "Engineering & Construction", Brand = Brand.GHDAdvisory, Price = 888.00m },
                new ProductDto { Id = 4, Name = "Environmental Services", Brand = Brand.eSolutionsGroup, Price = 888.00m }
            };
        }
    }
}
