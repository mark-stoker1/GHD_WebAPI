using GHD_WebAPI.Handlers.DTOs;
using MediatR;

namespace GHD_WebAPI.Handlers.QueryHandlers.Queries
{
    /// <summary>
    /// Product query request object, to get by Id.
    /// </summary>
    public class ProductQuery : IRequest<ProductDto>
    {
        public required int Id { get; set; }
    }
}
