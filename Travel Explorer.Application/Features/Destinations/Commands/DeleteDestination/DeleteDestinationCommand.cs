
namespace Travel_Explorer.Application.Features.Destinations.Commands.DeleteDestination
{

    /// <summary>
    /// Soft-deletes a destination by ID.
    /// </summary>
    public record DeleteDestinationCommand(int Id) : IRequest<bool>;

}
