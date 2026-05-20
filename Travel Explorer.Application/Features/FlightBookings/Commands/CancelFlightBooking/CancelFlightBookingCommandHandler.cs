using Travel_Explorer.Application.Common;
using Travel_Explorer.Domain.Enums;

namespace Travel_Explorer.Application.Features.FlightBookings.Commands.CancelFlightBooking
{
    public class CancelFlightBookingCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : IRequestHandler<CancelFlightBookingCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<bool> Handle(CancelFlightBookingCommand request, CancellationToken cancellationToken)
        {
            var spec = new FlightBookingSpecification(request.Id);
            var booking = await _unitOfWork.Repository<FlightBooking>().GenericEntitiesWithSpec(spec) ?? throw new NotFoundException(nameof(FlightBooking), request.Id);
            if (!_currentUserService.IsAdmin && booking.UserId != _currentUserService.UserId)
            {
                throw new ForbiddenAccessException();
            }

            if (booking.Status != BookingStatus.Cancelled && booking.Status != BookingStatus.Refunded)
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

                booking.Status = BookingStatus.Cancelled;
                booking.UpdatedAt = DateTime.UtcNow;

                _unitOfWork.Repository<FlightBooking>().Update(booking);
                await _unitOfWork.SaveChangesAsync();
            }

            return true;
        }
    }
}
