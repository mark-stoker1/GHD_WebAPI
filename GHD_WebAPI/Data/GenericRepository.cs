using GHD_WebAPI.Data.DataEntities;
using GHD_WebAPI.Data.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace GHD_WebAPI.Data
{
    /// <summary>
    /// Generic implementation for CRUD operations.
    /// Can be reused by custom repository implementations.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericRepository<T>(DbContext dbContext, ILogger<GenericRepository<T>> logger) : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly DbContext _dbContext = dbContext;
        protected readonly DbSet<T> _dbSet = dbContext.Set<T>();
        protected readonly ILogger<GenericRepository<T>> _logger = logger;

        public IQueryable<T> GetAll()
        {
            try
            {
                return _dbSet.Where(e => !e.IsDeleted);
            }
            catch (SqlException exception)
            {
                _logger.LogError(exception, exception.Message);
                throw;
            }
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                return await _dbSet.FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted, cancellationToken);
            }
            catch (SqlException exception)
            {
                _logger.LogError(exception, exception.Message);
                throw;
            }
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            try
            {
                await _dbSet.AddAsync(entity, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (SqlException exception)
            {
                _logger.LogError(exception, exception.Message);
                throw;
            }
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken)
        {
            try
            {
                if (entity is null) return;

                _dbSet.Update(entity);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (SqlException exception)
            {
                _logger.LogError(exception, exception.Message);
                throw;
            }
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            try
            {
                _dbSet.Update(entity);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (SqlException exception)
            {
                _logger.LogError(exception, exception.Message);
                throw;
            }
        }
    }
}
