using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.DTOs.Flights.Schedules;

namespace Travel_Explorer.Application.Features.Flights.Queries.GetAllFlightSchedules
{
    public class GetAllFlightSchedulesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetAllFlightSchedulesQuery, PaginatedResult<FlightScheduleDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<PaginatedResult<FlightScheduleDto>> Handle(
            GetAllFlightSchedulesQuery request, CancellationToken cancellationToken)
        {
            var p = request.Params;

            
            var dataSpec = new FlightScheduleSpecification(p);
            var totalCount = await _unitOfWork.Repository<FlightSchedule>().CountAsync(dataSpec);
            var flightSchedules = await _unitOfWork.Repository<FlightSchedule>().ListSpecAsync(dataSpec);

            var dtos = _mapper.Map<IReadOnlyList<FlightScheduleDto>>(flightSchedules);
            return new PaginatedResult<FlightScheduleDto>(dtos, totalCount, p.PageNumber, p.PageSize);
        }
    }
}
