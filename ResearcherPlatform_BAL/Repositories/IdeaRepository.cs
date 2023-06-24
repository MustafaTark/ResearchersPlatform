using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResearchersPlatform_BAL.Contracts;
using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_BAL.RepoExtentions;
using ResearchersPlatform_BAL.RequestFeatures;
using ResearchersPlatform_DAL.Data;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ResearchersPlatform_BAL.Repositories
{
    public class IdeaRepository : GenericRepository<Idea> , IIdeaRepository
    {
        private readonly IMapper _mapper;
        public IdeaRepository(AppDbContext context,IMapper mapper)
            :base(context)
        {
            _mapper= mapper;
        }
        public void CreateIdea(Idea idea, Guid creatorId)
        {
            var researcher = _context.Researchers.FirstOrDefault(r => r.Id == creatorId);
            idea.CreatorId = creatorId;
            idea.ParticipantsNumber = 1;
            idea.Participants.Add(researcher!);
            Create(idea);
        }
        public void UpdateIdea(Idea idea) => Update(idea);
        public void DeleteIdea(Idea idea) => Delete(idea);
        public async Task<IEnumerable<Idea?>> GetAllIdeasAsync(bool trackChanges)
            => await FindAll(trackChanges)
           
            .OrderBy(o => o.Name)
            .ToListAsync();
        public async Task<Idea?> GetIdeaAsync(Guid ideaId, bool trackChanges)
            => await FindByCondition(i => i.Id == ideaId , trackChanges)
            .Include(s => s.SpecalityObj)
            .Include(t => t.TopicObject)
            .Include(r => r.ResearcherCreator)
            .Include(p => p.Participants)
            .Include(t => t.Tasks)
            .OrderBy(o => o.Deadline) 
            .FirstOrDefaultAsync();
        public async Task<IEnumerable<Idea?>> GetAllIdeasForResearcherAsync(Guid researcherId, bool trackChanges)
            => await FindByCondition(i => i.Participants.FirstOrDefault(i => i.Id == researcherId)!.Id == researcherId , trackChanges)
            .Include(s => s.SpecalityObj)
            .Include(t => t.TopicObject)
            .Include(r => r.ResearcherCreator)
            .OrderBy(o => o.Deadline).ToListAsync();

        public async Task<IEnumerable<Idea?>> GetAllIdeasForCreatorAsync(Guid researcherId, bool trackChanges)
            => await FindByCondition(i => i.CreatorId == researcherId, trackChanges)
            .Include(s => s.SpecalityObj)
            .Include(t => t.TopicObject)
            .Include(r => r.ResearcherCreator)
            .OrderBy(o => o.Deadline).ToListAsync();
        public async Task<IEnumerable<IdeaDto>> GetAllIdeas(IdeasParamters paramters, bool trackChanges)
            => await FindAll(trackChanges)
            .ProjectTo<IdeaDto>(_mapper.ConfigurationProvider)
            .Search(paramters.SearchTerm!,paramters.Topic,paramters.Specality)
            .Skip((paramters.PageNumber - 1) * paramters.PageSize)
            .Take(paramters.PageSize)
            .OrderBy(o => o.Deadline).ToListAsync();
        public async Task<bool> ValidateIdeaCreation(Guid researcherId)
        {
            var researcherPoints = await _context.Set<Researcher>()
                .AsNoTracking()
                .Where(s => s.Id == researcherId)
                .Select(s => s.Points).FirstOrDefaultAsync();
            if(researcherPoints >= 4)
                return true;
            return false;
        }
        public async Task<IEnumerable<TopicsDto>> GetAvailableTopics(Guid researcherId)
        {
            var researcherPoints = await _context.Set<Researcher>().Where( r=> r.Id == researcherId)
                .AsNoTracking()
                .Select(s => s.Points)
                .FirstOrDefaultAsync();

            var topics = new List<TopicsDto>();
            if (researcherPoints > 4) {
                return await _context.Topics.ProjectTo<TopicsDto>(_mapper.ConfigurationProvider).ToListAsync();  
            }
            else
            {
                return await _context.Topics.ProjectTo<TopicsDto>(_mapper.ConfigurationProvider).Where(t => t.MinmumPoints <= 4).ToListAsync();
            }
        }
        public async Task<bool> ValidateResearcherForIdea(Guid ideaId,Guid researcherId)
        {
            var ideaParticipant = await FindByCondition(i => i.Id == ideaId 
            && i.Participants.FirstOrDefault(r => r.Id == researcherId)!.Id == researcherId, trackChanges:false)
                .FirstOrDefaultAsync();
            if (ideaParticipant is null)
                return false;
            return true;
        }
        public async Task<bool> HasParticipants(Guid ideaId)
        {
            var ideaParticipants = await _context.Ideas.Where(i => i.Id == ideaId && i.Participants.Any()).ToListAsync();
            if(ideaParticipants.Any()) return true;
            return false;
        }
        public async Task<bool> HasParticipantsMoreThanOne(Guid ideaId)
        {
            var participantNumber = await _context.Ideas.Where(i => i.Id == ideaId).Select(p => p.ParticipantsNumber).FirstOrDefaultAsync();
            if (participantNumber < 2) return true;
            return false;
        }
        public async Task<bool> CheckParticipantsNumber(Guid ideaId)
        {
            var idea = await FindByCondition(i => i.Id == ideaId , trackChanges:false)
                .Include(p => p.Participants).FirstOrDefaultAsync();

            if (idea!.ParticipantsNumber < idea.MaxParticipantsNumber) 
                return true;
            return false;
        }
        public async Task<bool> CheckResearcherIdeasNumber(Guid researcherId)
        {
            var ideas = await FindByCondition(i => i.CreatorId == researcherId, trackChanges: false).ToListAsync();
            if (ideas.Count >= 2)
                return false;
            return true;
        }
       
        public async Task Submit(Guid ideaId)
        {
            await AddRateToCreator(ideaId);
        }
        
        public async Task<bool> IsCreatorToIdea(Guid ideaId, Guid creatorId)
        {
            var idea = await FindByCondition(i => i.Id == ideaId && i.CreatorId==creatorId, trackChanges: false)
               .FirstOrDefaultAsync();
            if(idea != null)
                return true;
            return false;
        }
        private async Task AddRateToCreator(Guid ideaId)
        {
            var idea = await FindByCondition(i => i.Id == ideaId, trackChanges: false)
                .Select(i => new { i.CreatorId,i.Deadline})
                .FirstOrDefaultAsync();
            var researcher = await _context.Researchers.Where(r => r.Id == idea!.CreatorId).FirstOrDefaultAsync();
            bool isBeforeDeadline = DateTime.Now <= idea!.Deadline;
            if (isBeforeDeadline)
            {
                researcher!.SumRates += 10;
                researcher.TotalRates += 1;

                researcher.OverallRate = researcher.SumRates / researcher.TotalRates;
            }
            else
            {
                researcher!.SumRates += 5;
                researcher.TotalRates += 1;
                researcher.OverallRate = researcher.SumRates / researcher.TotalRates;
            }
        }

    }
}
