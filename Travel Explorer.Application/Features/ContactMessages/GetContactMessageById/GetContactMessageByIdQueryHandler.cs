using Travel_Explorer.Application.DTOs.ContactMessage;

namespace Travel_Explorer.Application.Features.ContactMessages.GetContactMessageById
{
    public class GetContactMessageByIdQueryHandler 
        : IRequestHandler<GetContactMessageByIdQuery, ContactMessageDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetContactMessageByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ContactMessageDto?> Handle(
            GetContactMessageByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new ContactMessageSpecification(request.Id);
            var message = await _unitOfWork.Repository<ContactMessage>().GenericEntitiesWithSpec(spec);

            if (message == null)
                throw new NotFoundException(nameof(ContactMessage), request.Id);


            // If a requesting user ID is provided, validate ownership
            if (request.RequestingUserId.HasValue && message.UserId != request.RequestingUserId.Value)
                throw new NotFoundException(nameof(ContactMessage), request.Id);


            return _mapper.Map<ContactMessageDto>(message);
        }
    }
}
