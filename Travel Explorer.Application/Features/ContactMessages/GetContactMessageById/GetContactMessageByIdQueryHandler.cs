using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.DTOs.ContactMessage;

namespace Travel_Explorer.Application.Features.ContactMessages.GetContactMessageById
{
    public class GetContactMessageByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
                : IRequestHandler<GetContactMessageByIdQuery, ContactMessageDto?>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ContactMessageDto?> Handle(
            GetContactMessageByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new ContactMessageSpecification(request.Id);
            var message = await _unitOfWork.Repository<ContactMessage>().GenericEntitiesWithSpec(spec) ?? throw new NotFoundException(nameof(ContactMessage), request.Id);

            // If the user is not an Admin, they must be the owner of the message
            if (!_currentUserService.IsAdmin && message.UserId != _currentUserService.UserId)
                throw new NotFoundException(nameof(ContactMessage), request.Id);

            return _mapper.Map<ContactMessageDto>(message);
        }
    }
}
