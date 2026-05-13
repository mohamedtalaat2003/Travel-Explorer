
namespace Travel_Explorer.Application.Features.Activities.Commands.DeleteActivity
{
    public class DeleteActivityCommandHandler
        : IRequestHandler<DeleteActivityCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteActivityCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            DeleteActivityCommand request, CancellationToken cancellationToken)
        {
            var activity = await _unitOfWork.Repository<Activity>().GetAsync(request.Id);

            if (activity == null)
                throw new NotFoundException(nameof(Activity), request.Id);

            activity.IsDeleted = true;
            activity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        
    }
}
}
