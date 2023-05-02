using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_BAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Contracts
{
    public interface IChatRepository
    {
        Task<IEnumerable<MessageDto>> GetMessagesToIdea(Guid ideaId);
        Task<IEnumerable<MessageDto>> GetMessagesToTasks(Guid taskId);
       void CreateIdeaMessage(Guid ideaId, Guid researcherId, MessageViewModel messageVM);
        void CreateTaskMessage(Guid taskId, Guid researcherId, MessageViewModel messageVM);
    }
}
