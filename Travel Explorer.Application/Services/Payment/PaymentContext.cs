using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_Explorer.Application.DTOs;

namespace Travel_Explorer.Application.Services.Payment
{
    public class PaymentContext
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "EGP";
        public string MerchantOrderId { get; set; } = string.Empty;
        public BillingDataDto Billing { get; set; } 
    }
}
