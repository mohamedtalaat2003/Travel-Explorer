
namespace Travel_Explorer.Application.Features.Destinations.Queries.SearchDestinations
{

    /// <summary>
    /// Searches destinations by keyword, location, price range, and/or category.
    /// </summary>
    public record SearchDestinationsQuery(
        string? Keyword,
        string? Location,
        decimal? MinPrice,
        decimal? MaxPrice,
        int? CategoryId) : IRequest<IReadOnlyList<DestinationDto>>;

}
