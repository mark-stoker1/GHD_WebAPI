using GHD_WebAPI.Data.Interfaces;
using GHD_WebAPI.Handlers.DTOs;
using GHD_WebAPI.Handlers.QueryHandlers.Queries;
using GHD_WebAPI.Mappers;
using MediatR;

namespace GHD_WebAPI.Handlers.QueryHandlers
{
    /// <summary>
    /// Handler for retrieving a product by its ID.
    /// </summary>
    public class GetProductByIdQueryHandler : IRequestHandler<ProductQuery, ProductDto?>
    {
        private readonly IProductsRepository _productsRepository;
        private readonly ILogger<GetProductByIdQueryHandler> _logger;

        /// <summary>
        /// Initializes required dependencies for the handler.
        /// </summary>
        /// <param name="productsRepository"></param>
        /// <param name="logger"></param>
        public GetProductByIdQueryHandler(IProductsRepository productsRepository, ILogger<GetProductByIdQueryHandler> logger)
        {
            _productsRepository = productsRepository;
            _logger = logger;
        }

        /// <summary>
        /// Handles the retrieval of a product by its ID.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>ProductDto</returns>
        public async Task<ProductDto?> Handle(ProductQuery query, CancellationToken cancellationToken)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(query, nameof(query));

                var product = await _productsRepository.GetByIdAsync(query.Id, cancellationToken);

                if (product is null) { return null; }

                return product.MapToDto();
            }
            catch (ArgumentNullException exception)
            {
                _logger.LogError(exception, exception.Message);
                throw;
            }
        }
    }
}
