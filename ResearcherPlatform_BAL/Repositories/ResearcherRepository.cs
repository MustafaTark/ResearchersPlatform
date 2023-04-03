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
    public class ResearcherRepository : GenericRepository<Researcher> , IResearcherRepository
    {
        public ResearcherRepository(AppDbContext context):base(context)
        {

        }
        public void UpdateResearcher(Researcher researcher) => Update(researcher);
        public void DeleteResearcher(Researcher researcher) => Delete(researcher);
        public async Task<Researcher?> GetResearcherByIdAsync(string researcherId, bool trackChanges)
            => await FindByCondition(r => r.Id == researcherId, trackChanges)
            .Include(i => i.Ideas)
            .Include(i => i.Points)
            .Include(i => i.Badges)
            .Include(i => i.Level)
            .FirstOrDefaultAsync();
        public async Task<IEnumerable<Researcher?>> GetAllResearchersAsync(bool trackChanges)
            => await FindAll(trackChanges)
            .ToListAsync();
    }
}
