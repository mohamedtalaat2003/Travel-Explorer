using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_Explorer.Application.DTOs.Users
{
    public class LoginDto
    {

        [Required(ErrorMessage = "Username is empty")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is empty")]
        public string Password { get; set; } = string.Empty;
    }
}
