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
    public class SectionRepository : GenericRepository<Section>,ISectionRepository
    {
        private readonly IMapper _mapper;
        public SectionRepository(AppDbContext context,IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }
        public async Task<IEnumerable<SectionDto>> GetSectionsToCourse(Guid courseId)
           =>await FindByCondition(s=>s.CourseId==courseId,trackChanges:false)
                .ProjectTo<SectionDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        
        public void CreateSectionsToCourse(Guid courseId,List<Section> sections)
        {
           foreach(var section in sections) {

                _context.Courses.Where(c => c.Id == courseId).SingleOrDefault()!.Sections.Add(section);
            }
        }

        public async Task<SectionDto?> GetSectionByIdAsync(Guid sectionId, bool trackChanges)
        {
           
          var section=  await FindByCondition(s => s.Id == sectionId, trackChanges)
           .ProjectTo<SectionDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            return section;
        }
        
    }
}
