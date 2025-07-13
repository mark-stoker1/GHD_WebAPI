using GHD_WebAPI.Data.DataEntities;
using GHD_WebAPI.Data.Interfaces;
using GHD_WebAPI.Handlers.CommandHandlers.Commands;
using GHD_WebAPI.Handlers.DTOs;
using GHD_WebAPI.Mappers;
using MediatR;

namespace GHD_WebAPI.Handlers.CommandHandlers
{
    /// <summary>
    /// Handler for creating a new product.
    /// Initializes dependencies for the CreateProductCommandHandler.
    /// </remarks>
    /// <param name="productsRepository"></param>
    /// <param name="logger"></param>
    public class CreateProductCommandHandler(IProductsRepository productsRepository, ILogger<CreateProductCommandHandler> logger) : IRequestHandler<CreateProductCommand, (bool success, ProductDto? product)>
    {
        private readonly IProductsRepository _productsRepository = productsRepository;
        private readonly ILogger<CreateProductCommandHandler> _logger = logger;

        /// <summary>
        /// Handles the creation of a new product.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<(bool success, ProductDto? product)> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(command, nameof(command));
                ArgumentNullException.ThrowIfNull(command?.Name, nameof(command.Name));

                var productExists = await _productsRepository.ProductExistsAsync(command.Name, command.Brand.ToString(), cancellationToken);

                if (productExists)
                {
                    _logger.LogWarning("Create failed: Product '{Name}' with brand '{Brand}' already exists.", command.Name, command.Brand);
                    return (false, null);
                }

                var product = new Product
                {
                    Name = command.Name,
                    Brand = command.Brand.ToString(),
                    Price = command.Price
                };

                await _productsRepository.AddAsync(product, cancellationToken);
                return (true, product.MapToDto());
            }
            catch (ArgumentNullException exception)
            {
                _logger.LogError(exception, exception.Message);
                throw;
            }
        }
    }
}
