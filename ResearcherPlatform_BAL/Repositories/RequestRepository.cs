using AutoMapper;
using Azure.Core;
using ResearchersPlatform_BAL.Contracts;
using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_DAL.Data;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Repositories
{
    public class RequestRepository : GenericRepository<RequestIdea> , IRequestRepository
    {
        private readonly IMapper _mapper;
        public RequestRepository(AppDbContext context,IMapper mapper):base(context)
        {
            _mapper = mapper;
        }
        public void CreateRequest(Guid ideaId, RequestIdea request)
        {
            var idea = _context.Ideas.FirstOrDefault(i => i.Id == ideaId);
            //_context.Set<RequestIdea>().FirstOrDefault().Add(request);
        }

        public void DeleteRequest(RequestIdea request)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RequestDto>> GetAllRequests(Guid ideaId, bool trackChanges)
        {
            throw new NotImplementedException();
        }

        public Task<RequestDto> GetRequestByIdeaId(Guid ideaId, bool trackChanges)
        {
            throw new NotImplementedException();
        }

        public Task<RequestDto> GetRequestByRequestId(Guid requestId, bool trackChanges)
        {
            throw new NotImplementedException();
        }
        //void DeleteRequest(RequestIdea request);

        //Task<RequestDto> GetRequestByIdeaId(Guid ideaId, bool trackChanges);
        //Task<RequestDto> GetRequestByRequestId(Guid requestId, bool trackChanges);
        //Task<IEnumerable<RequestDto>> GetAllRequests(Guid ideaId, bool trackChanges);
    }
}
