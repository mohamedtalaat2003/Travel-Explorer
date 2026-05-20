using Travel_Explorer.Application.Common;
using Travel_Explorer.Domain.Enums;

namespace Travel_Explorer.Application.Features.DestinationBookings.Commands.CancelBooking
{
    public class CancelDestinationBookingCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : IRequestHandler<CancelDestinationBookingCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<bool> Handle(CancelDestinationBookingCommand request, CancellationToken cancellationToken)
        {
            var spec = new DestinationBookingSpecification(request.Id);
            var booking = await _unitOfWork.Repository<DestinationBooking>().GenericEntitiesWithSpec(spec) ?? throw new NotFoundException(nameof(DestinationBooking), request.Id);
            if (!_currentUserService.IsAdmin && booking.UserId != _currentUserService.UserId)
            {
                throw new ForbiddenAccessException();
            }

            if (booking.Status != BookingStatus.Cancelled && booking.Status != BookingStatus.Refunded)
            {
                booking.Status = BookingStatus.Cancelled;
                booking.UpdatedAt = DateTime.UtcNow;

                _unitOfWork.Repository<DestinationBooking>().Update(booking);
                await _unitOfWork.SaveChangesAsync();
            }

            return true;
        }
    }
}
