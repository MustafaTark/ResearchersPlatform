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
    public class PaperRepository : GenericRepository<Paper> , IPaperRepository
    {
        private readonly IMapper _mapper;
        public PaperRepository(AppDbContext context,IMapper mapper):base(context)
        {
            _mapper = mapper;

        }
    
        public void CreatePaper(Paper paper , Guid researcherId) 
            => _context.Set<Researcher>().FirstOrDefault(r => r.Id == researcherId)!.Papers.Add(paper);
        public void UpdatePaper(Paper paper) => Update(paper);
        public void DeletePaper(Paper paper) => Delete(paper);

        public async Task<PaperDto?> GetPaperById(Guid paperId, bool trackChanges)
            => await FindByCondition(p => p.Id == paperId, trackChanges)
            .ProjectTo<PaperDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        public async Task<Paper?> GetPaperByIdForDeletion(Guid paperId, bool trackChanges)
            => await FindByCondition(p => p.Id == paperId, trackChanges)
            .FirstOrDefaultAsync();
        public async Task<IEnumerable<PaperDto?>> GetAllPapers(bool trackChanges)
            => await FindAll(trackChanges)
            .ProjectTo<PaperDto>(_mapper.ConfigurationProvider)
            .OrderBy(p=> p.Name)
            .ToListAsync();
    }
}
