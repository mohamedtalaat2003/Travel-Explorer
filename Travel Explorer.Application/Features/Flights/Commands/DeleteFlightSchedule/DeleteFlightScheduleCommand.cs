namespace Travel_Explorer.Application.Features.Flights.Commands.DeleteFlightSchedule
{
    public record DeleteFlightScheduleCommand(int Id) : IRequest<bool>;
}
