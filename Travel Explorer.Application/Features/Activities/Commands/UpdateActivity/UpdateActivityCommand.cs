
using System.Text.Json.Serialization;

namespace Travel_Explorer.Application.Features.Activities.Commands.UpdateActivity
{
    /// <summary>
    /// Updates an existing activity.
    /// </summary>
    public record UpdateActivityCommand(
        string Name,
        string Description,
        string Icon,
        List<string> ImageUrls,
        int DestinationId) : IRequest<ActivityDto?>
    {
        [JsonIgnore]
        public int Id { get; set; }
    }
}
