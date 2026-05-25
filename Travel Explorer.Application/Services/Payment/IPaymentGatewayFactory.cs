using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_Explorer.Application.Services.Payment
{
    public interface IPaymentGatewayFactory
    {
        IPaymentGateway GetGateway(string providerName);
    }
}
