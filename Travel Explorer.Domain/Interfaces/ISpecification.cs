using System.Linq.Expressions;

namespace Travel_Explorer.Domain.Interfaces
{
    public interface ISpecification<T> where T : class
    {
        Expression<Func<T, bool>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }

        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }

        int Take { get; }
        int Skip { get; }
        bool HasPaging { get; }

        bool IsSplitQuery { get; }
    }
}
