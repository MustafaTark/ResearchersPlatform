using ResearchersPlatform_BAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Contracts
{
    public interface IChatClient
    {
        Task ReceiveMessage(MessageViewModel message);
    }
}
