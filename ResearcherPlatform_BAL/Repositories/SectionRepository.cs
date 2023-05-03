using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _memoryCache;
        public SectionRepository(AppDbContext context,IMapper mapper, IMemoryCache memoryCache) : base(context)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
        }
        public async Task<IEnumerable<SectionDto>> GetSectionsToCourse(Guid courseId)
              => await FindByCondition(s => s.CourseId == courseId, trackChanges: false)
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
            string key = $"section:{sectionId}";
            var sections = await _memoryCache.GetOrCreateAsync(
                key,
                async entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
                    return await FindByCondition(s => s.Id == sectionId, trackChanges)
                               .ProjectTo<SectionDto>(_mapper.ConfigurationProvider)
                               .FirstOrDefaultAsync();
                }
              );
            return sections!;
        }
        
    }
}
