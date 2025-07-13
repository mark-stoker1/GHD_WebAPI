using GHD_WebAPI.Data.DataEntities;

namespace GHD_WebAPI.Data.Interfaces
{
    /// <summary>
    /// Generic interface to be used by all custom repository implementations.T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericRepository<T> where T : notnull, BaseEntity
    {
        IQueryable<T> GetAll();
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task AddAsync(T entity, CancellationToken cancellationToken);
        Task UpdateAsync(T entity, CancellationToken cancellationToken);
        Task DeleteAsync(T entity, CancellationToken cancellationToken);
    }
}
