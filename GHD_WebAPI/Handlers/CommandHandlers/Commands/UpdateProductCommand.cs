using GHD_WebAPI.Enums;
using GHD_WebAPI.Handlers.DTOs;
using MediatR;

namespace GHD_WebAPI.Handlers.CommandHandlers.Commands
{
    /// <summary>
    /// Update Product Command.
    /// </summary>
    public class UpdateProductCommand : IRequest<(bool success, string? error, ProductDto? product)>
    {
        public required int Id { get; set; }
        public required string Name { get; set; } = string.Empty;
        public required Brand Brand { get; set; }
        public required decimal Price { get; set; }
    }
}
