using GHD_WebAPI.Enums;

namespace GHD_WebAPI.Handlers.DTOs
{
    /// <summary>
    /// DTO object to be returned with CRUD operations.
    /// </summary>
    public class ProductDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; } = string.Empty;
        public required Brand Brand { get; set; }
        public required decimal Price { get; set; }
        public string? SelfLink { get; set; }
    }
}
