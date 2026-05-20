using Travel_Explorer.Application.DTOs.Flights.Schedules;

namespace Travel_Explorer.Application.Features.Flights.Commands.UpdateFlightSchedule
{
    public record UpdateFlightScheduleCommand(int Id, UpdateFlightScheduleDto Dto) : IRequest<FlightScheduleDto>;
}
