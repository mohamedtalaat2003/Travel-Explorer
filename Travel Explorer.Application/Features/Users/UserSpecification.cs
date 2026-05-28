using System;
using System.Linq.Expressions;
using Travel_Explorer.Application.Common.Parameters;
using Travel_Explorer.Domain.Common;
using Travel_Explorer.Domain.Entities;

namespace Travel_Explorer.Application.Features.Users
{
    public class UserSpecification : BaseSpecification<ApplicationUser>
    {
        public UserSpecification(UserSpecParams parameters)
        {
            // Filter out deleted users
            AddCriteria(u => !u.IsDeleted);

            // Filtering
            if (parameters.Gender.HasValue)
            {
                AddCriteria(u => u.Gender == parameters.Gender.Value);
            }

            if (parameters.IsBlocked.HasValue)
            {
                AddCriteria(u => u.IsBlocked == parameters.IsBlocked.Value);
            }

            if (parameters.Status.HasValue)
            {
                AddCriteria(u => u.Status == parameters.Status.Value);
            }

            // Searching
            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var search = parameters.SearchTerm.Trim().ToLower();
                AddCriteria(u =>
                    u.UserName!.ToLower().Contains(search) ||
                    u.Email!.ToLower().Contains(search) ||
                    u.FullName.ToLower().Contains(search));
            }

            // Pagination
            ApplyPaging((parameters.PageNumber - 1) * parameters.PageSize, parameters.PageSize);
        }

        public UserSpecification(int id)
        {
            AddCriteria(u => u.Id == id && !u.IsDeleted);
        }
    }
}
