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
    public class SpecialityRepository : GenericRepository<Specality>, ISpecialityRepository
    {
        private readonly IMapper _mapper;
        public SpecialityRepository(AppDbContext context,IMapper mapper):base(context)
        {
            _mapper = mapper;
        }

        public void CreateSpeciality(Specality specality) => Create(specality);

        public async Task<IEnumerable<SpecialityDto?>> GetAllSpecialites(bool trackChanges)
            => await FindAll(trackChanges)       
            .ProjectTo<SpecialityDto>(_mapper.ConfigurationProvider)
            .ToListAsync();


        public async Task<SpecialityDto?> GetSpecialityById(int specialityId, bool trackChanges)
            => await FindByCondition(s=> s.Id == specialityId,trackChanges)
            .ProjectTo<SpecialityDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }
}
