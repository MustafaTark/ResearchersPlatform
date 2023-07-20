using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Contracts
{
    public interface IEmailService
    {
       Task SendPasswordResetEmailAsync(string email, string callbackUrl);
    }
}
    