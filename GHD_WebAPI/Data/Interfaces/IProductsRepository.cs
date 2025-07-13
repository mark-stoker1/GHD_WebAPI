using GHD_WebAPI.Data.DataEntities;

namespace GHD_WebAPI.Data.Interfaces
{
    /// <summary>
    /// Extends IGenericRepository and extends with custom methods for Products domain.a
    /// </summary>
    public interface IProductsRepository : IGenericRepository<Product>
    {
        Task<bool> ProductExistsAsync(string name, string brand, CancellationToken cancellationToken);
    }
}
