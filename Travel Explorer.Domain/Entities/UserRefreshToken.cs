using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_Explorer.Domain.Entities
{
    public class UserRefreshToken :BaseEntity
    {
        public string  TokenHash { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public string? DeviceInfo { get; set; }

        [ForeignKey("User")]
        public int? UserId { get; set; }
        public ApplicationUser? User { get; set; } 


    }
}
