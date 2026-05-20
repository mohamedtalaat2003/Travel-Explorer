using System.Text.Json.Serialization;
using Travel_Explorer.Application.DTOs.Flights.Schedules;

namespace Travel_Explorer.Application.Features.Flights.Commands.UpdateFlightSchedule
{
    public record UpdateFlightScheduleCommand(
        string Airline,
        string FlightNumber,
        string DepartureCity,
        string ArrivalCity,
        DateTime DepartureTime,
        DateTime ArrivalTime,
        decimal EconomyPrice,
        decimal BusinessPrice,
        decimal FirstClassPrice,
        int AvailableEconomySeats,
        int AvailableBusinessSeats,
        int AvailableFirstClassSeats
    ) : IRequest<FlightScheduleDto>
    {
        [JsonIgnore]
        public int Id { get; set; }
    }
}
