using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ResearchersPlatform_BAL.Contracts;
using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_BAL.ViewModels;
using ResearchersPlatform_DAL.Data;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Repositories
{
    public class ResearcherRepository : GenericRepository<Researcher>, IResearcherRepository
    {
        private readonly IMapper _mapper;
        public ResearcherRepository(AppDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }
        public void DeleteResearcher(Researcher researcher) => Delete(researcher);
        public async Task<ResearcherViewModel?> GetSingleResearcherByIdAsync(Guid researcherId, bool trackChanges)
        {
            var resercher = await FindByCondition(r => r.Id == researcherId, trackChanges)
            .Include(i => i.Ideas)
            .Include(i => i.IdeasLeader)
            .Include(i => i.Papers)
            .Include(i => i.SpecalityObject)
            .ProjectTo<ResearcherDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
            var student = await _context.Students
                .Include(s => s.Badges)
                .Where(s => s.Id == resercher!.StudentId)
                .Select(s => new { Id = s.Id, Badges = s.Badges, FirstName = s.Firstname, LastName = s.Lastname })
                .FirstOrDefaultAsync();
            var resercherVM = new ResearcherViewModel
            {
                Id = resercher!.Id,
                FirstName = student!.FirstName,
                LastName = student.LastName,
                StudentId = student.Id,
                Ideas = resercher.Ideas,
                Points = resercher.Points,
                Level = resercher.Level,
                IdeasLeader = resercher!.IdeasLeader,
                Papers = resercher!.Papers,
                Specality = resercher.SpecalityObject,
                Badges = student!.Badges,//can be exception
            };
            return resercherVM;
        }
        public async Task<IEnumerable<Researcher?>> GetAllResearchersAsync(bool trackChanges)
            => await FindAll(trackChanges)
            .ToListAsync();
        public void CreateSpecality(int specality,string studentId)
        {
           var researcher= FindByCondition(r=>r.StudentId==studentId,trackChanges:true).FirstOrDefault();
            researcher!.SpecalityId= specality;
        }
        public void CreatePapersToResearcher(Guid researcherId, List<Paper> papers)
        {
            var researcher =  FindByCondition(s => s.Id == researcherId, trackChanges: true)
                                  .FirstOrDefault();
            foreach (var paper in papers)
            {
                researcher!.Papers.Add(paper);
            }
            int points = papers.Count / 2;
           var pointscheck=researcher!.Points+points;
            researcher.Level = pointscheck switch
            {
                1 or 2 or 3 => Level.Beginner,
                4 or 5 or 6 => Level.Intermediate,
                7 or 8 => Level.Professional,
                _ => Level.Expert // this is the default case
            };
            researcher!.Points += points;
        }
        public async void AddSpeciality(Guid researcherId,int specialityId)
        {
            var researcher = await FindByCondition(r => r.Id == researcherId, trackChanges: true)
                .FirstOrDefaultAsync();
                    researcher!.SpecalityId = specialityId;
        }
        public async Task<Researcher?> GetResearcherByIdAsync(Guid researcherId, bool trackChanges)
            => await FindByCondition(r => r.Id == researcherId , trackChanges)
            .Include(i => i.Ideas)
            .FirstOrDefaultAsync();

        public void DeleteSpeciality(Guid researcherId)
        {
            var researcher = _context.Researchers.Where(r => r.Id == researcherId).FirstOrDefault();
            researcher!.SpecalityId = 1;
            _context.SaveChangesAsync();
        }
         public async Task<IEnumerable<SingleResearcherDto>> GetAllIdeaParticipants(Guid ideaId)
             => await FindByCondition(i => i.Ideas.FirstOrDefault(i => i.Id== ideaId)!.Id == ideaId, trackChanges: false)
             .ProjectTo<SingleResearcherDto>(_mapper.ConfigurationProvider)
             .ToListAsync();
        public async Task<string?> GetResearcherByStudentId(string studentId)
            => await FindByCondition(r => r.StudentId == studentId,trackChanges:false).Select(r => r.Id.ToString()).FirstOrDefaultAsync();

    }
}
    
 
