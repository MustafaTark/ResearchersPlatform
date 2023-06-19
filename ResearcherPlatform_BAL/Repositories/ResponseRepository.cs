using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure;
using Microsoft.EntityFrameworkCore;
using ResearchersPlatform_BAL.Contracts;
using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Repositories
{
    public class ResponseRepository : GenericRepository<ResearchersPlatform_DAL.Models.Response>,IResponseRepository
    {
        private readonly IMapper _mapper;
        public ResponseRepository(AppDbContext context , IMapper mapper):base(context)
        {
            _mapper = mapper;
        }
        public async Task<IEnumerable<ResponseDto>> GetAllResponses(bool trackChanges)
            => await FindAll(trackChanges)
            .ProjectTo<ResponseDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        public async Task<ResponseDto?> GetResponseById(bool trackChanges, Guid responseId)
            => await FindByCondition(r => r.Id == responseId, trackChanges)
            .ProjectTo<ResponseDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        public async Task<ResearchersPlatform_DAL.Models.Response?> GetSingleResponseById(Guid responseId)
            => await FindByCondition(r => r.Id == responseId,trackChanges:false)
            .FirstOrDefaultAsync();

        public async Task<IEnumerable<ResponseDto>> GetAllStudentResponses(bool trackChanges, string studentId)
            => await FindByCondition(s => s.StudentId == studentId, trackChanges)
            .ProjectTo<ResponseDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        public void CreateResponse(ResearchersPlatform_DAL.Models.Response response) => Create(response);
        public void DeleteResponse(ResearchersPlatform_DAL.Models.Response response) => Delete(response);
    }
}
