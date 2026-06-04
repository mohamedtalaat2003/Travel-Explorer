using Travel_Explorer.Application.DTOs.Flights.Bookings;
using Travel_Explorer.Domain.Enums;

namespace Travel_Explorer.Application.Features.FlightBookings.Commands.UpdateFlightBookingStatus
{
    public class UpdateFlightBookingStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateFlightBookingStatusCommand, FlightBookingDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<FlightBookingDto> Handle(UpdateFlightBookingStatusCommand request, CancellationToken cancellationToken)
        {
            var spec = new FlightBookingSpecification(request.Id);
            var booking = await _unitOfWork.Repository<FlightBooking>().GenericEntitiesWithSpec(spec) ?? throw new NotFoundException(nameof(FlightBooking), request.Id);

            
            if ((request.Status == BookingStatus.Cancelled || request.Status == BookingStatus.Refunded) &&
                booking.Status != BookingStatus.Cancelled && booking.Status != BookingStatus.Refunded)
            {
                var flightSchedule = await _unitOfWork.Repository<FlightSchedule>().GetAsync(booking.FlightScheduleId);
                if (flightSchedule != null)
                {
                    switch (booking.Class)
                    {
                        case FlightClass.Economy:
                            flightSchedule.AvailableEconomySeats += booking.NumberOfPassengers;
                            break;
                        case FlightClass.Business:
                            flightSchedule.AvailableBusinessSeats += booking.NumberOfPassengers;
                            break;
                        case FlightClass.FirstClass:
                            flightSchedule.AvailableFirstClassSeats += booking.NumberOfPassengers;
                            break;
                    }
                    _unitOfWork.Repository<FlightSchedule>().Update(flightSchedule);
                }
            }

            booking.Status = request.Status;
            booking.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Repository<FlightBooking>().Update(booking);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<FlightBookingDto>(booking);
        }
    }
}
