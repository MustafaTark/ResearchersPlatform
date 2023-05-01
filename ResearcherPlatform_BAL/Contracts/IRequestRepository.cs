using Azure.Core;
using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Contracts
{
    public interface IRequestRepository
    {
        Task SendRequest(Guid researcherId, Guid ideaId);
        void DeleteRequest(Guid requestId, Guid researcherId);
        Task<RequestDto?> GetRequestByIdeaId(Guid ideaId,bool trackChanges);
        Task<RequestDto?> GetRequestById(Guid requestId,bool trackChanges);
        Task<IEnumerable<RequestDto>> GetAllRequests(Guid ideaId,bool trackChanges);
        Task<IEnumerable<RequestDto>> GetAllRequestsForResearcher(Guid researcherId, bool trackChanges);
        bool ValidateRequest(Guid researcherId, Guid ideaId);
        Task<bool> ValidateIdea(Guid ideaId);
        Task<bool> ValidateResearcher(Guid researcherId, Guid ideaId);
        Task AcceptRequest(Guid requestId, Guid researcherId);


    }
}
