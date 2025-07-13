using GHD_WebAPI.Enums;
using GHD_WebAPI.Handlers.DTOs;
using MediatR;
using System.ComponentModel;

namespace GHD_WebAPI.Handlers.QueryHandlers.Queries
{
    /// <summary>
    /// Products Query request object.
    /// </summary>
    public class ProductsQuery : IRequest<IList<ProductDto>>
    {
        [DefaultValue(1)]
        public required int Page { get; init; } = 1;
        [DefaultValue(10)]
        public required int PageSize { get; init; } = 10;
        public string? Name { get; init; }
        public Brand? Brand { get; init; }
    }
}
