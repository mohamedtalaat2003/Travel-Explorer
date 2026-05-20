using Travel_Explorer.Application.DTOs.Flights.Schedules;

namespace Travel_Explorer.Application.Features.Flights.Queries.GetFlightScheduleById
{
    public record GetFlightScheduleByIdQuery(int Id) : IRequest<FlightScheduleDto>;
}
