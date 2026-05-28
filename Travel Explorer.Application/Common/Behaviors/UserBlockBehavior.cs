
namespace Travel_Explorer.Application.Common.Behaviors
{
    public class UserBlockBehavior<TRequest, TResponse>(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            
            if (userId.HasValue && userId.Value > 0)
            {
                var spec = new BaseSpecification<ApplicationUser>(u => u.Id == userId.Value);
                var user = await _unitOfWork.Repository<ApplicationUser>().GenericEntitiesWithSpec(spec);

                if (user != null && (user.IsBlocked || user.IsDeleted))
                {
                    throw new UnauthorizedAccessException("This account not active in system");
                }
            }

            return await next();
        }
    }
}