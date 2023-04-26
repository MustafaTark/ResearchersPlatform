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
        void CreateRequest(Guid ideaId, RequestIdea request);
        void DeleteRequest(RequestIdea request);

        Task<RequestDto> GetRequestByIdeaId(Guid ideaId,bool trackChanges);
        Task<RequestDto> GetRequestByRequestId(Guid requestId,bool trackChanges);
        Task<IEnumerable<RequestDto>> GetAllRequests(Guid ideaId,bool trackChanges);
    }
}
