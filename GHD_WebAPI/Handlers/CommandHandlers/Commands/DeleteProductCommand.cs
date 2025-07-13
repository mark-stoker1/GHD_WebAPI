using MediatR;

namespace GHD_WebAPI.Handlers.CommandHandlers.Commands
{
    /// <summary>
    /// Command to delete a product by its ID.
    /// </summary>
    public class DeleteProductCommand : IRequest<bool>
    {
        public required int Id { get; set; }
    }
}
