using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.DTOs.ContactMessage;

namespace Travel_Explorer.Application.Features.ContactMessages.GetAllContactMessages
{
    public class GetAllContactMessagesQueryHandler 
        : IRequestHandler<GetAllContactMessagesQuery, PaginatedResult<ContactMessageDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllContactMessagesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<ContactMessageDto>> Handle(
            GetAllContactMessagesQuery request, CancellationToken cancellationToken)
        {
            var spec = new ContactMessageSpecification(request.PageNumber, request.PageSize, request.IsRead, request.UserId);
            
            var totalCount = await _unitOfWork.Repository<ContactMessage>().CountAsync(spec);
            var data = await _unitOfWork.Repository<ContactMessage>().ListSpecAsync(spec);

            var dtos = _mapper.Map<IReadOnlyList<ContactMessageDto>>(data);

            return new PaginatedResult<ContactMessageDto>(dtos, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
