using GHD_WebAPI.Enums;
using GHD_WebAPI.Handlers.DTOs;
using MediatR;

namespace GHD_WebAPI.Handlers.CommandHandlers.Commands
{
    /// <summary>
    /// Command to create a new product.
    /// </summary>
    public class CreateProductCommand : IRequest<(bool success, ProductDto? productDto)>
    {
        public required string Name { get; set; } = string.Empty;
        public required Brand Brand { get; set; }
        public required decimal Price { get; set; }
    }
}
