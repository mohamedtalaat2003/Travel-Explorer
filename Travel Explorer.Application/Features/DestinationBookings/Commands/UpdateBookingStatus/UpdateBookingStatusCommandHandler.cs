using Travel_Explorer.Domain.Enums;

namespace Travel_Explorer.Application.Features.DestinationBookings.Commands.UpdateBookingStatus
{
    public class UpdateBookingStatusCommandHandler
        : IRequestHandler<UpdateBookingStatusCommand, DestinationBookingDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateBookingStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DestinationBookingDto?> Handle(
            UpdateBookingStatusCommand request, CancellationToken cancellationToken)
        {
            var spec = new DestinationBookingSpecification(request.Id);
            var booking = await _unitOfWork.Repository<DestinationBooking>().GenericEntitiesWithSpec(spec);

            if (booking == null)
                return null;

            booking.Status = request.Status;
            booking.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var reloadSpec = new DestinationBookingSpecification(booking.Id);
            var loaded = await _unitOfWork.Repository<DestinationBooking>().GenericEntitiesWithSpec(reloadSpec);

            return _mapper.Map<DestinationBookingDto>(loaded);
        
    }
}
}
