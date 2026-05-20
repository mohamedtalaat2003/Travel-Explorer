using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.Common.Parameters;
using Travel_Explorer.Application.DTOs.Flights.Schedules;

namespace Travel_Explorer.Application.Features.Flights.Queries.GetAllFlightSchedules
{
    public record GetAllFlightSchedulesQuery(FlightScheduleSpecParams Params) : IRequest<PaginatedResult<FlightScheduleDto>>;
}
