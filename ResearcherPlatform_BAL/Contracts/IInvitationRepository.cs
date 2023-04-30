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
        //void CreateInvitationForIdea(Guid ideaId);
        Task<InvitationDto?> GetInvitationById(Guid invitationid,bool trackChanges);
        Task<InvitationDto?> GetInvitationByIdeaId(Guid ideaId, bool trackChanges);
        Task<IEnumerable<InvitationDto?>> GetAllInvitationsForResearcher(Guid researcherId,bool trackChanges);

        void CreateInvitations(Guid ideaId , List<Guid> researcherIds);
        Task SendInvitation(Guid invitationId , Guid ideaId, List<Guid> researcherIds);
        Task<bool> ValidationAcception(Guid invitationId, Guid researcherId);
        Task AcceptInvitation(Guid invitationId, Guid researcherId);
        Task<bool> ValidationInvitation(Guid invitationId, Guid researcherId);
        void DeleteInvitation(Guid invitationId, Guid researcherId);

    }
}
