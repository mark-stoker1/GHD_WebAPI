using GHD_WebAPI.Data.DataEntities;
using GHD_WebAPI.Data.Interfaces;
using GHD_WebAPI.Handlers.DTOs;
using GHD_WebAPI.Handlers.QueryHandlers.Queries;
using GHD_WebAPI.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GHD_WebAPI.Handlers.QueryHandlers
{
    /// <summary>
    /// QueryHandler to get all products.
    /// </summary>
    public class GetAllProductsQueryHandler(IProductsRepository productsRepository, ILogger<GetAllProductsQueryHandler> logger) : IRequestHandler<ProductsQuery, IList<ProductDto>>
    {
        private readonly IProductsRepository _productsRepository = productsRepository;
        private readonly ILogger<GetAllProductsQueryHandler> _logger = logger;

        /// <summary>
        /// Handle method for calling data layer, paging and filtering and returnsing DTO object.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>IList of ProductDto</returns>
        public async Task<IList<ProductDto>> Handle(ProductsQuery query, CancellationToken cancellationToken)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(query, nameof(query));

                var products = await ApplyFilters(_productsRepository.GetAll(), query)
                    .Skip((query.Page - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .ToListAsync(cancellationToken);

                if (products == null || !products.Any()) { return new List<ProductDto>(); }

                return products.MapToDtoList();
            }
            catch (ArgumentNullException exception)
            {
                _logger.LogError(exception, exception.Message);
                throw;
            }
        }

        private static IQueryable<Product> ApplyFilters(IQueryable<Product> products, ProductsQuery query)
        {
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                products = products.Where(p => p.Name.Contains(query.Name));
            }

            if (!string.IsNullOrWhiteSpace(query.Brand.ToString()))
            {
                products = products.Where(p => p.Brand == query.Brand.ToString());
            }

            return products;
        }
    }
}
