﻿using AutoMapper;
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
    public class IdeaRepository : GenericRepository<Idea> , IIdeaRepository
    {
        private readonly IMapper _mapper;
        public IdeaRepository(AppDbContext context,IMapper mapper)
            :base(context)
        {
            _mapper= mapper;
        }
        public void CreateIdea(Idea idea) => Create(idea);
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
    }
}
