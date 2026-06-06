using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_Explorer.Application.Services
{
    public class SpecificationUserRefreshToken :BaseSpecification<UserRefreshToken>
    {
        public SpecificationUserRefreshToken(string tokenHash):base(rt => rt.TokenHash == tokenHash)
        {
            AddInclude(a => a.User);
        }
        public SpecificationUserRefreshToken(string tokenHash , int userId):base(rt => rt.TokenHash == tokenHash && rt.UserId == userId
                    && !rt.IsRevoked)
        {
            AddInclude(a => a.User);
        }
        public SpecificationUserRefreshToken(int userId):base( rt => rt.UserId == userId
                    && !rt.IsRevoked)
        {
            AddInclude(a => a.User);
        }
    }
}
