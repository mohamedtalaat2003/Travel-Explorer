using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_Explorer.Application.DTOs.Account
{
    public class RefreshTokenRequestDto
    {
        public required string RefreshToken { get; set; }
    }
}
