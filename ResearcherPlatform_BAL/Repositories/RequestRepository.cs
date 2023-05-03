using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
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
        public bool ValidateRequest(Guid researcherId, Guid ideaId)
        {
            var researcherRequest = _context.Requests.Any(r => r.IdeaId == ideaId && r.ResearcherId == researcherId);
            if (researcherRequest)
                return true;
            return false;
        }
        public async Task<bool> ValidateIdea(Guid ideaId)
        {
            var idea = await _context.Ideas.Where(i => i.Id == ideaId).FirstOrDefaultAsync();
            if(idea!.ParticipantsNumber < idea.MaxParticipantsNumber)
                return true;
            return false;
        }
        public async Task<bool> ValidateResearcher(Guid researcherId , Guid ideaId)
        {
            var ideaCreator = await _context.Ideas
                .Where(i => i.Id == ideaId)
                .Select(i=>i.CreatorId)
                .FirstOrDefaultAsync();
            if (ideaCreator == researcherId)
                return false;
            return true;
        }
        public async Task SendRequest(Guid researcherId, Guid ideaId)
        {
            var idea = await _context.Ideas.FirstOrDefaultAsync(i => i.Id== ideaId);
            var researcher = await _context.Researchers.FirstOrDefaultAsync(r => r.Id == researcherId);
            //if (!ValidateRequest(researcherId, ideaId) && ValidateIdea(ideaId) && ValidateResearcher(researcherId,ideaId)) { 
                var request = new RequestIdea { IdeaId = ideaId, ResearcherId = researcherId };
                _context.Requests.Add(request);
        }
        private bool ValidateAcception(Guid requestId, Guid researcherId)
        {
            var request = FindByCondition(i => i.Id == requestId
              && i.ResearcherId == researcherId
              && i.IsAccepted == false, trackChanges: false).FirstOrDefaultAsync();
            if (request is null)
                return false;
            return true;
        }
        public async Task AcceptRequest(Guid requestId, Guid researcherId)
        {
            var researcher = await _context.Researchers.Include(r => r.Requests).FirstOrDefaultAsync(r => r.Id == researcherId);
            var request =  researcher!.Requests.FirstOrDefault(r => r.Id == requestId);
            var idea = await _context.Ideas.FirstOrDefaultAsync(i => i.Id == request!.IdeaId);
            if(ValidateAcception(requestId, researcherId))
            {
                request!.IsAccepted = true;
                _context.Set<Idea>().FirstOrDefault(i => i.Id == idea!.Id)!.Participants.Add(researcher);
                idea!.ParticipantsNumber++;
                DeleteRequest(requestId, researcherId);
            }

        }
        public void DeleteRequest(Guid requestId, Guid researcherId)
        {
            var request = FindByCondition(i => i.Id == requestId && i.ResearcherId == researcherId, trackChanges: true).FirstOrDefault();
            _context.Requests.Remove(request!);
        }
        public async Task<IEnumerable<RequestDto>> GetAllRequests(Guid ideaId, bool trackChanges)
            => await FindByCondition(i => i.IdeaId == ideaId , trackChanges:false)
            .ProjectTo<RequestDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        public async Task<RequestDto?> GetRequestByIdeaId(Guid ideaId, bool trackChanges)
            => await FindByCondition(r => r.IdeaId == ideaId, trackChanges: false)
            .ProjectTo<RequestDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        public async Task<RequestDto?> GetRequestById(Guid requestId, bool trackChanges)
            => await FindByCondition(r => r.Id == requestId, trackChanges: false)
            .ProjectTo<RequestDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        public async Task<IEnumerable<RequestDto>> GetAllRequestsForResearcher(Guid researcherId, bool trackChanges)
            => await FindByCondition(r => r.ResearcherId == researcherId, trackChanges: false)
            .ProjectTo<RequestDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }
}
