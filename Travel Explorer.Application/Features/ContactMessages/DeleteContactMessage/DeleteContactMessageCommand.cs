namespace Travel_Explorer.Application.Features.ContactMessages.DeleteContactMessage
{
    public record DeleteContactMessageCommand(int Id) : IRequest<bool>;

    public class DeleteContactMessageCommandHandler : IRequestHandler<DeleteContactMessageCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteContactMessageCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteContactMessageCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<ContactMessage>();
            var message = await repo.GetAsync(request.Id);

            if (message == null) return false;

            await repo.Delete(request.Id);
            return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
