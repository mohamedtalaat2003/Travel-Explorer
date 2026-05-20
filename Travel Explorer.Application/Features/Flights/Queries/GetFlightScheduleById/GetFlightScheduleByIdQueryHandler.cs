using Travel_Explorer.Application.DTOs.Flights.Schedules;

namespace Travel_Explorer.Application.Features.Flights.Queries.GetFlightScheduleById
{
    public class GetFlightScheduleByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetFlightScheduleByIdQuery, FlightScheduleDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<FlightScheduleDto> Handle(GetFlightScheduleByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new FlightScheduleSpecification(request.Id);
            var flightSchedule = await _unitOfWork.Repository<FlightSchedule>().GenericEntitiesWithSpec(spec) ?? throw new NotFoundException(nameof(FlightSchedule), request.Id);
            return _mapper.Map<FlightScheduleDto>(flightSchedule);
        }
    }
}
