using ResearchersPlatform_BAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Contracts
{
    public interface IAuthService
    {
        Task<bool> ValidateUser(UserForLoginDto userForAuth);
        Task<string> CreateToken();
    }
}
