using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Contracts
{
    public interface IInvitationRepository
    {
        void CreateInvitation(Guid ideaid ,Guid creatorId, Invitation invitation);
        Task<InvitationDto?> GetInvitationById(Guid invitationid,bool trackChanges);
        Task<InvitationDto?> GetInvitationByIdeaId(Guid ideaId, bool trackChanges);
        Task<IEnumerable<InvitationDto?>> GetAllInvitationsForResearcher(Guid researcherId,bool trackChanges);

    }
}
