using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_Explorer.Application.DTOs
{
    public class AssignRoleDto
    {
        public int? userId { get; set; }
        public string? newRole { get; set; }
    }
}
