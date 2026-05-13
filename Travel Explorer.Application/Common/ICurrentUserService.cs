using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_Explorer.Application.Common
{
    public interface ICurrentUserService
    {
        int? UserId { get; }
    }
}
