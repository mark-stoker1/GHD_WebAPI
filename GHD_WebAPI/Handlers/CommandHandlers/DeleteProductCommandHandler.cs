using GHD_WebAPI.Data.Interfaces;
using GHD_WebAPI.Handlers.CommandHandlers.Commands;
using MediatR;

namespace GHD_WebAPI.Handlers.CommandHandlers
{
    /// <summary>
    /// Command Handler for the deletion of a product.
    /// </summary>
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IProductsRepository _productsRepository;
        private readonly ILogger<DeleteProductCommandHandler> _logger;

        /// <summary>
        /// Initializes dependencies for the command handler.
        /// </summary>
        /// <param name="productsRepository"></param>
        /// <param name="logger"></param>
        public DeleteProductCommandHandler(IProductsRepository productsRepository, ILogger<DeleteProductCommandHandler> logger)
        {
            _productsRepository = productsRepository;
            _logger = logger;
        }

        /// <summary>
        /// Handles the deletion of a product by its ID.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>bool</returns>
        public async Task<bool> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(command, nameof(command));

                var product = await _productsRepository.GetByIdAsync(command.Id, cancellationToken);

                if (product is null)
                {
                    _logger.LogWarning("Delete failed: Product with ID {Id} not found.", command.Id);
                    return false;
                }

                product.IsDeleted = true;
                await _productsRepository.DeleteAsync(product, cancellationToken);

                return true;
            }
            catch (ArgumentNullException exception)
            {
                _logger.LogError(exception, exception.Message);
                throw;
            }
        }
    }
}
