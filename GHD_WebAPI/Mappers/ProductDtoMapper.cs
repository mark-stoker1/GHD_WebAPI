using GHD_WebAPI.Data.DataEntities;
using GHD_WebAPI.Enums;
using GHD_WebAPI.Handlers.DTOs;

namespace GHD_WebAPI.Mappers
{
    public static class ProductDtoMapper
    {
        public static ProductDto MapToDto(this Product product)
        {
            ArgumentNullException.ThrowIfNull(product, nameof(product));

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Brand = Enum.Parse<Brand>(product.Brand),
                Price = product.Price
            };
        }

        public static List<ProductDto> MapToDtoList(this IEnumerable<Product> products)
        {
            ArgumentNullException.ThrowIfNull(products, nameof(products));

            return products.Select(p => p.MapToDto()).ToList();
        }
    }
}
