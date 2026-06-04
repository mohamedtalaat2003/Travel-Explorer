using Travel_Explorer.Application.Common;

namespace Travel_Explorer.Application.Features.DestinationBookings.Commands.CreateBooking
{
    public class CreateBookingCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
                : IRequestHandler<CreateBookingCommand, DestinationBookingDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<DestinationBookingDto> Handle(
            CreateBookingCommand request, CancellationToken cancellationToken)
        {
            
            var destination = await _unitOfWork.Repository<Destination>().GetAsync(request.DestinationId) ?? throw new NotFoundException(nameof(Destination), request.DestinationId);
            var nights = (request.CheckOutDate.Date - request.CheckInDate.Date).Days;
            if (nights <= 0)
                throw new BadRequestException("Check-out date must be after check-in date.");

            var booking = _mapper.Map<DestinationBooking>(request);
            booking.UserId = _currentUserService.UserId ?? 0;
            booking.TotalPrice = destination.PricePerNight * nights;
            booking.Status = Domain.Enums.BookingStatus.Pending;
            booking.CreatedAt = DateTime.UtcNow;

            
            booking.CheckInDate = DateTime.SpecifyKind(request.CheckInDate, DateTimeKind.Utc);
            booking.CheckOutDate = DateTime.SpecifyKind(request.CheckOutDate, DateTimeKind.Utc);

            await _unitOfWork.Repository<DestinationBooking>().AddAsync(booking);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            
            var spec = new DestinationBookingSpecification(booking.Id);
            var loaded = await _unitOfWork.Repository<DestinationBooking>().GenericEntitiesWithSpec(spec);

            return _mapper.Map<DestinationBookingDto>(loaded);
        
    }
}
}
