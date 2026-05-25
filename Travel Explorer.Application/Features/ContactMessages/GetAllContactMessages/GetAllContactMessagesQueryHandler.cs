using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.DTOs.ContactMessage;

namespace Travel_Explorer.Application.Features.ContactMessages.GetAllContactMessages
{
    public class GetAllContactMessagesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
                : IRequestHandler<GetAllContactMessagesQuery, PaginatedResult<ContactMessageDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<PaginatedResult<ContactMessageDto>> Handle(
            GetAllContactMessagesQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.IsAdmin ? null : _currentUserService.UserId;
            var spec = new ContactMessageSpecification(request.Params, userId);
            
            var totalCount = await _unitOfWork.Repository<ContactMessage>().CountAsync(spec);
            var data = await _unitOfWork.Repository<ContactMessage>().ListSpecAsync(spec);

            var dtos = _mapper.Map<IReadOnlyList<ContactMessageDto>>(data);

            return new PaginatedResult<ContactMessageDto>(dtos, totalCount, request.Params.PageNumber, request.Params.PageSize);
        }
    }
}
