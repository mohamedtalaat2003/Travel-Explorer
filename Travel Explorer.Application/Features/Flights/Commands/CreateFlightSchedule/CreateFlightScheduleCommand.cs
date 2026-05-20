using Travel_Explorer.Application.DTOs.Flights.Schedules;

namespace Travel_Explorer.Application.Features.Flights.Commands.CreateFlightSchedule
{
    public record CreateFlightScheduleCommand(CreateFlightScheduleDto Dto) : IRequest<FlightScheduleDto>;
}
