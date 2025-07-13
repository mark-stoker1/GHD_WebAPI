using GHD_WebAPI.Data.DataEntities;
using GHD_WebAPI.Data.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace GHD_WebAPI.Data
{
    /// <summary>
    /// Repository with custom methods.
    /// Implements the generic repository for generic GetAll, GetByIdAsync, AddAsync, UpdateAsync, DeleteAsync.
    /// </summary>
    public class ProductsRepository(
        ProductsDbContext productsDbContext,
        ILogger<ProductsRepository> productsLogger) : GenericRepository<Product>(productsDbContext, productsLogger), IProductsRepository
    {
        private readonly ProductsDbContext _productsDbContext = productsDbContext;
        private readonly ILogger<ProductsRepository> _productsLogger = productsLogger;

        public async Task<bool> ProductExistsAsync(string name, string brand, CancellationToken cancellationToken)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(name, nameof(name));
                ArgumentNullException.ThrowIfNull(brand, nameof(brand));

                return await _productsDbContext.Products.AnyAsync(p =>
                    p.Name == name &&
                    p.Brand == brand &&
                    !p.IsDeleted,
                    cancellationToken);
            }
            catch (ArgumentNullException exception)
            {
                _productsLogger.LogError(exception, exception.Message);
                throw;
            }
            catch (SqlException exception)
            {
                _productsLogger.LogError(exception, exception.Message);
                throw;
            }
        }
    }
}
