
namespace Travel_Explorer.Application.Features.Activities.Commands.CreateActivity
{
    
    
    
    public record CreateActivityCommand(
        string Name,
        string Description,
        string Icon,
        List<string> ImageUrls,
        int DestinationId) : IRequest<ActivityDto>;
}
