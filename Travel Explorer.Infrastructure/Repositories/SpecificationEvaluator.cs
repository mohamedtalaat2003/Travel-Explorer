using Microsoft.EntityFrameworkCore;
using Travel_Explorer.Domain.Interfaces;

namespace Travel_Explorer.Infrastructure.Repositories
{
    public class SpecificationEvaluator<T> where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
        {
            var query = inputQuery;

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            if (spec.HasPaging)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            if (spec.IsSplitQuery)
            {
                query = query.AsSplitQuery();
            }
            // Apply includes for navigation properties
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }
}

