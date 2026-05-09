
namespace Travel_Explorer.Application.Features.DestinationBookings.Commands.CreateBooking
{
    public class CreateBookingCommandHandler
        : IRequestHandler<CreateBookingCommand, DestinationBookingDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateBookingCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DestinationBookingDto> Handle(
            CreateBookingCommand request, CancellationToken cancellationToken)
        {
            // Fetch destination to calculate total price
            var destination = await _unitOfWork.Repository<Destination>().GetAsync(request.DestinationId);
            if (destination == null)
                throw new KeyNotFoundException($"Destination with ID {request.DestinationId} not found.");

            var nights = (request.CheckOutDate.Date - request.CheckInDate.Date).Days;
            if (nights <= 0)
                throw new ArgumentException("Check-out date must be after check-in date.");

            var booking = _mapper.Map<DestinationBooking>(request);
            booking.TotalPrice = destination.PricePerNight * nights;
            booking.Status = Domain.Enums.BookingStatus.Pending;
            booking.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<DestinationBooking>().AddAsync(booking);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Reload with includes for the response
            var spec = new DestinationBookingSpecification(booking.Id);
            var loaded = await _unitOfWork.Repository<DestinationBooking>().GenericEntitiesWithSpec(spec);

            return _mapper.Map<DestinationBookingDto>(loaded);
        
    }
}
}
