using GHD_WebAPI.Data.Interfaces;
using GHD_WebAPI.Handlers.CommandHandlers.Commands;
using GHD_WebAPI.Handlers.DTOs;
using GHD_WebAPI.Mappers;
using MediatR;

namespace GHD_WebAPI.Handlers.CommandHandlers
{
    /// <summary>
    /// Command Handler for updating a product.
    /// </summary>
    /// <remarks>
    /// Initializes dependencies for the command handler.
    /// </remarks>
    /// <param name="productsRepository"></param>
    /// <param name="logger"></param>
    public class UpdateProductCommandHandler(IProductsRepository productsRepository, ILogger<UpdateProductCommandHandler> logger) : IRequestHandler<UpdateProductCommand, (bool success, string? error, ProductDto? product)>
    {
        private readonly IProductsRepository _productsRepository = productsRepository;
        private readonly ILogger<UpdateProductCommandHandler> _logger = logger;

        /// <summary>
        /// Handles the update of a product by its ID.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<(bool success, string? error, ProductDto? product)> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(command, nameof(command));
                ArgumentNullException.ThrowIfNull(command?.Name, nameof(command.Name));

                // Can only update existing product.
                var existingProduct = await _productsRepository.GetByIdAsync(command.Id, cancellationToken);
                if (existingProduct is null)
                {
                    _logger.LogWarning("Update failed: Product with ID {Id} not found.", command.Id);
                    return (false, $"Product with ID {command.Id} not found.", null);
                }

                // If the Product and Brand combination already exists, return an error.
                var productAndBrandExists = await _productsRepository.ProductExistsAsync(command.Name, command.Brand.ToString(), cancellationToken);
                if (productAndBrandExists)
                {
                    _logger.LogWarning("Update failed: Product '{Name}' with brand '{Brand}' already exists.", command.Name, command.Brand);
                    return (false, $"Product '{command.Name}' with brand '{command.Brand}' already exists.", null);
                }

                existingProduct.Name = command.Name;
                existingProduct.Brand = command.Brand.ToString();
                existingProduct.Price = command.Price;

                await _productsRepository.UpdateAsync(existingProduct, cancellationToken);

                return (true, null, existingProduct.MapToDto());
            }
            catch (ArgumentNullException exception)
            {
                _logger.LogError(exception, exception.Message);
                throw;
            }
        }
    }
}
