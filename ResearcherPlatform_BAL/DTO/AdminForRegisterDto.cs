using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class AdminForRegisterDto
    {

        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string UserName { get; set; }
        public required string PhoneNumber { get; set; }
    }
}
