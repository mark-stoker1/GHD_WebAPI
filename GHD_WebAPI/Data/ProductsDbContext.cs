using GHD_WebAPI.Data.DataEntities;
using Microsoft.EntityFrameworkCore;

namespace GHD_WebAPI.Data
{
    /// <summary>
    /// DB Context implementation for Products.
    /// Contains in-memory DB for Products.
    /// </summary>
    /// <param name="options"></param>
    public class ProductsDbContext(DbContextOptions<ProductsDbContext> options) : DbContext(options)
    {
        public virtual DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(p =>
            {
                p.HasKey(x => x.Id);
                // This index is just a unique check
                // However, in a real implementation when we lookup on these fields, a Clustered Index would be more efficient.
                p.HasIndex(p => new { p.Name, p.Brand }).IsUnique();
                p.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
                p.Property(x => x.Name).IsRequired().HasMaxLength(100);
                p.Property(x => x.Brand).IsRequired().HasMaxLength(100);
                p.Property(x => x.Price).IsRequired().HasColumnType("decimal(18,2)");
                p.Property(x => x.IsDeleted).IsRequired();
                p.Property(x => x.CreatedAt).IsRequired();
                p.Property(x => x.UpdatedAt).IsRequired(false);
                p.HasData(
                    new Product { Id = 1, Name = "Advisory Services", Brand = "GHDWoodhead", Price = 100.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 2, Name = "Architecture & Design", Brand = "GHDWoodhead", Price = 55.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 3, Name = "Engineering & Construction", Brand = "GHDWoodhead", Price = 71.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 4, Name = "Environmental Services", Brand = "GHDWoodhead", Price = 1000.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 5, Name = "Digital Solutions", Brand = "GHDWoodhead", Price = 5555.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 6, Name = "Energy & Resources", Brand = "GHDWoodhead", Price = 999.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 7, Name = "Transportation", Brand = "GHDWoodhead", Price = 2111.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 8, Name = "Water Services", Brand = "GHDWoodhead", Price = 666.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 9, Name = "Advisory Services", Brand = "GHDDigital", Price = 888.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 10, Name = "Architecture & Design", Brand = "GHDDigital", Price = 888.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 11, Name = "Engineering & Construction", Brand = "GHDDigital", Price = 10.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 12, Name = "Environmental Services", Brand = "GHDDigital", Price = 111.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 13, Name = "Digital Solutions", Brand = "GHDDigital", Price = 88.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 14, Name = "Energy & Resources", Brand = "GHDDigital", Price = 666.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 15, Name = "Transportation", Brand = "GHDDigital", Price = 555.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 16, Name = "Water Services", Brand = "GHDDigital", Price = 444.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 17, Name = "Advisory Services", Brand = "GHDAdvisory", Price = 888.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 18, Name = "Architecture & Design", Brand = "GHDAdvisory", Price = 777.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 19, Name = "Engineering & Construction", Brand = "GHDAdvisory", Price = 888.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 20, Name = "Environmental Services", Brand = "GHDAdvisory", Price = 888.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 21, Name = "Digital Solutions", Brand = "GHDAdvisory", Price = 888.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 22, Name = "Energy & Resources", Brand = "GHDAdvisory", Price = 666.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 23, Name = "Transportation", Brand = "GHDAdvisory", Price = 888.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 24, Name = "Water Services", Brand = "GHDAdvisory", Price = 666.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 25, Name = "Advisory Services", Brand = "eSolutionsGroup", Price = 888.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 26, Name = "Architecture & Design", Brand = "eSolutionsGroup", Price = 555.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 27, Name = "Engineering & Construction", Brand = "eSolutionsGroup", Price = 444.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 28, Name = "Environmental Services", Brand = "eSolutionsGroup", Price = 888.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 29, Name = "Digital Solutions", Brand = "eSolutionsGroup", Price = 777.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 30, Name = "Energy & Resources", Brand = "eSolutionsGroup", Price = 666.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 31, Name = "Transportation", Brand = "eSolutionsGroup", Price = 555.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 32, Name = "Water Services", Brand = "eSolutionsGroup", Price = 777.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 33, Name = "Advisory Services", Brand = "MovementStrategies", Price = 333.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 34, Name = "Architecture & Design", Brand = "MovementStrategies", Price = 550.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 35, Name = "Engineering & Construction", Brand = "MovementStrategies", Price = 1444.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 36, Name = "Environmental Services", Brand = "MovementStrategies", Price = 1888.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 37, Name = "Digital Solutions", Brand = "MovementStrategies", Price = 1055.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 38, Name = "Energy & Resources", Brand = "MovementStrategies", Price = 10.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 39, Name = "Transportation", Brand = "MovementStrategies", Price = 10545.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 40, Name = "Water Services", Brand = "MovementStrategies", Price = 10898.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 41, Name = "Advisory Services", Brand = "OlssonFireAndRisk", Price = 7777.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 42, Name = "Architecture & Design", Brand = "OlssonFireAndRisk", Price = 555.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 43, Name = "Engineering & Construction", Brand = "OlssonFireAndRisk", Price = 555.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 44, Name = "Environmental Services", Brand = "OlssonFireAndRisk", Price = 555.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 45, Name = "Digital Solutions", Brand = "OlssonFireAndRisk", Price = 8888.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 46, Name = "Energy & Resources", Brand = "OlssonFireAndRisk", Price = 777.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 47, Name = "Transportation", Brand = "OlssonFireAndRisk", Price = 888.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                    new Product { Id = 48, Name = "Water Services", Brand = "OlssonFireAndRisk", Price = 888.00m, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = null });
            });
        }

        /// <summary>
        /// Override SaveChangesAsync to set UpdatedAt for modified Product entities.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>int</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var updatedEntries = ChangeTracker
                .Entries<Product>()
                .Where(e => e.State == EntityState.Modified);

            foreach (var entry in updatedEntries)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
