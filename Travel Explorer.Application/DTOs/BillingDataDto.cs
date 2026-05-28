using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_Explorer.Application.DTOs
{
    public class BillingDataDto
    {
        [Range(1.0, 1000000.0)]
        public decimal Amount { get; set; }

        //details for credit card
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
    }
}
