using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public class ExpertRequestRepository : GenericRepository<ExpertRequest> , IExpertRequestRepository
    {
        private readonly IMapper _mapper;
        public ExpertRequestRepository(AppDbContext context , IMapper mapper):base(context)
        {
            _mapper = mapper;
        }
        private bool ValidateRequest(Guid ideaId, Guid participantId)
        {
            var ideaParticipant = _context.Ideas.Where(i => i.Id == ideaId 
            && i.Participants.FirstOrDefault(r => r.Id == participantId)!.Id==participantId);
            if(ideaParticipant is not null)
                return true;
            return false;
        }
        public async Task<bool> ValidateParticipantPoints(Guid participantId)
        {
            var points = await _context.Researchers
                .Where(r => r.Id == participantId)
                .Select(r => r.Points)
                .FirstOrDefaultAsync();

            return points >= 3;
        }
        public void CreateRequest(ExpertRequest request) => Create(request);
        public async Task<ExpertRequestDto?> GetRequestById(Guid requestId, bool trackChanges)
            => await FindByCondition(i => i.Id == requestId, trackChanges)
            .ProjectTo<ExpertRequestDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        public async Task<IEnumerable<ExpertRequestDto>> GetAllRequestsByIdeaId(Guid ideaId, bool trackChanges)
            => await FindByCondition(i => i.IdeaId == ideaId, trackChanges)
            .ProjectTo<ExpertRequestDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        public async Task<IEnumerable<ExpertRequestDto>> GetAllRequestsForResearcher(Guid researcherId, bool trackChanges)
            => await FindByCondition(i => i.ParticipantId == researcherId, trackChanges)
            .ProjectTo<ExpertRequestDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        public async Task<IEnumerable<ExpertRequestDto>> GetAllRequestsForExpert(bool trackChanges)
            => await FindAll(trackChanges)
            .ProjectTo<ExpertRequestDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        public void DeleteRequest(ExpertRequest request) => Delete(request);
        public async Task<IEnumerable<ExpertRequestDto>> GetAllExpertRequests()
            => await FindAll(trackChanges:false)
            .ProjectTo<ExpertRequestDto>(_mapper.ConfigurationProvider).OrderBy(o => o.IdeaId).ToListAsync();
    }
}
