using System.Linq.Expressions;

namespace Travel_Explorer.Domain.Common
{
    public class BaseSpecification<T> : ISpecification<T> where T : class
    {
        public BaseSpecification() { } 

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
        
        public Expression<Func<T, bool>> Criteria { get; private set; }

        public List<Expression<Func<T, object>>> Includes { get; } = []; 

        public Expression<Func<T, object>> OrderBy { get; private set; }

        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        public int Take { get; private set; }
        public int Skip { get; private set; }

        public bool HasPaging { get; private set; }

        public bool IsSplitQuery { get; private set; }

        protected void ApplySplitQuery()
        {
            IsSplitQuery = true;
        }

        protected void AddCriteria(Expression<Func<T, bool>> criteriaExpression)
        {
            if (Criteria == null)
            {
                Criteria = criteriaExpression;
            }
            else
            {
                
                var parameter = Expression.Parameter(typeof(T));
                var combined = Expression.Lambda<Func<T, bool>>(
                    Expression.AndAlso(
                        Expression.Invoke(Criteria, parameter),
                        Expression.Invoke(criteriaExpression, parameter)
                    ), parameter);
                    
                Criteria = combined;
            }
        }


        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }

        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            HasPaging = true;
        }

        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
    }
}
