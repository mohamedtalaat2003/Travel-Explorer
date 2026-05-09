
namespace Travel_Explorer.Application.Features.DestinationBookings.Queries.GetBookingById
{
    public class GetBookingByIdQueryHandler
        : IRequestHandler<GetBookingByIdQuery, DestinationBookingDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBookingByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DestinationBookingDto?> Handle(
            GetBookingByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new DestinationBookingSpecification(request.Id);
            var booking = await _unitOfWork.Repository<DestinationBooking>().GenericEntitiesWithSpec(spec);

            if (booking == null)
                return null;

            return _mapper.Map<DestinationBookingDto>(booking);
        
    }
}
}
