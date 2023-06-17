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
    public class ProblemRepository : GenericRepository<Problem> , IProblemRepository
    {
        private readonly IMapper _mapper;
        public ProblemRepository(AppDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }
        public void CreateProblem(Problem problem)
             => Create(problem);
        public async Task<IEnumerable<ProblemDto>> GetProblemsAsync(int categoryId)
            => await FindByCondition(p=>p.ProblemCategoryId==categoryId,trackChanges:false)
                    .ProjectTo<ProblemDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();
        public async Task<ProblemDto?> GetProblemByIdAsync(Guid id)
          =>await 
            FindByCondition(p=>p.Id==id, trackChanges:false)
            .Include(p=>p.ProblemCategory)
            .ProjectTo<ProblemDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        public async Task<IEnumerable<ProblemCategory>> GetProblemCategories()
        {
            return await _context.ProblemCategories.ToListAsync();
        }
    }
}
