using System;
using System.Collections.Generic;
using System.Linq;
using Travel_Explorer.Application.Services.Payment;

namespace Travel_Explorer.Infrastructure.Services.Payment
{
    public class PaymentGatewayFactory : IPaymentGatewayFactory
    {
        private readonly Dictionary<string, IPaymentGateway> _gateways;

        public PaymentGatewayFactory(IEnumerable<IPaymentGateway> gateways)
        {
            _gateways = gateways.ToDictionary(g => g.ProviderName, g => g, StringComparer.OrdinalIgnoreCase);
        }

        public IPaymentGateway GetGateway(string providerName)
        {
            if (_gateways.TryGetValue(providerName, out var gateway))
                return gateway;

            throw new NotSupportedException($"Payment provider '{providerName}' is not registered.");
        }
    }
}
