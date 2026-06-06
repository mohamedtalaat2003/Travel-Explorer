using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_Explorer.Application.Services.Payment
{
    public class PaymobtSettings
    {
        public string PublicKey { get; set; } = string.Empty;
        public string Secretkey { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public string IFrameId { get; set; } = string.Empty;
        public string CardIntegrationId { get; set; } = string.Empty;
        public string HmacSecret { get; set; } = string.Empty;
        public string Currency { get; set; } = "EGP";
        public string PaymentMethodId { get; set; } = string.Empty;
    }
}
