using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Contracts
{
    public interface IExpertRequestRepository
    {
        void CreateRequest(ExpertRequest request);
        void DeleteRequest(ExpertRequest request);
        Task<ExpertRequestDto?> GetRequestById(Guid requestId , bool trackChanges);
        Task<IEnumerable<ExpertRequestDto>> GetAllRequestsByIdeaId(Guid ideaId, bool trackChanges);
        Task<IEnumerable<ExpertRequestDto>> GetAllRequestsForResearcher(Guid researcherId,bool trackChanges);
        Task<IEnumerable<ExpertRequestDto>> GetAllRequestsForExpert(bool trackChanges);
        Task<bool> ValidateParticipantPoints(Guid participantId);

    }
}
