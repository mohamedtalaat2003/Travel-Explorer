using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Travel_Explorer.Domain.Interfaces;

namespace Travel_Explorer.Domain.Common
{
    public class BaseSpecification<T> : ISpecification<T> where T : class
    {
        public BaseSpecification() { } //علشان لو في داتا هتيجي من غير معمل filterليها 

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
        //just get : علشان امنع من برا التغير 
        public Expression<Func<T, bool>> Criteria { get; }

        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>(); // just used to store navigations

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
        //Helper method to add include expressions to the specification 
        //make code cleaner and more maintainable and more easy to reade and avoid code duplication when we need to add include expressions in different specifications

        //protected to encapsulate the logic from the outside like controller
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
    }
}
