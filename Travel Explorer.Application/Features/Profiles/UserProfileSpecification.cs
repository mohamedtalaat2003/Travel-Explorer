using Travel_Explorer.Domain.Common;

namespace Travel_Explorer.Application.Features.Profiles
{
    public class UserProfileSpecification : BaseSpecification<UserProfile>
    {
        public UserProfileSpecification(int userId) 
            : base(p => p.UserId == userId)
        {
            AddInclude(p => p.User);
        }
    }
}
