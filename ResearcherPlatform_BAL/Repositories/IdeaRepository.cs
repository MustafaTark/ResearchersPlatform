using Microsoft.EntityFrameworkCore;
using ResearchersPlatform_BAL.Contracts;
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
        public IdeaRepository(AppDbContext context)
            :base(context)
        {

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
            .Include(t => t.Tasks)
            .Include(p => p.Participants)
            .OrderBy(o => o.Deadline) 
            .FirstOrDefaultAsync();
        public async Task<IEnumerable<Idea?>> GetAllIdeasForResearcherAsync(Guid researcherId, bool trackChanges)
            => await FindByCondition(i => i.Participants.FirstOrDefault(i => i.Id == researcherId)!.Id == researcherId , trackChanges)
            .OrderBy(o => o.Deadline).ToListAsync();
    }
}
