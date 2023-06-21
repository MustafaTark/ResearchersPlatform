using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ResearchersPlatform_BAL.Contracts;
using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_BAL.RepoExtentions;
using ResearchersPlatform_BAL.RequestFeatures;
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
             .Include(i => i.SpecalityObject)
            .Include(i => i.Papers)
            .ProjectTo<ResearcherDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
            var student = await _context.Students
               
                .Where(s => s.Id == resercher!.StudentId)
                .Include(s => s.Badges)
                .ProjectTo<StudentDto>(_mapper.ConfigurationProvider)
                //.Select(s => new { Id = s.Id, Badges = s.Badges, FirstName = s.Firstname, LastName = s.Lastname })
                .FirstOrDefaultAsync();
            var resercherVM = new ResearcherViewModel
            {
                Id = resercher!.Id,
                FirstName = student!.FirstName,
                LastName = student.LastName,
                StudentId = student.Id!,
                Points = resercher.Points,
                Level = resercher.Level,
                Papers = resercher!.Papers,
                Specality = resercher.SpecalityObject,
                Badges = student!.Badges,
            };
            return resercherVM;
        }
        public async Task<IEnumerable<Researcher?>> GetAllResearchersAsync(ResearcherParamters paramters,bool trackChanges)
            => await FindAll(trackChanges)
            .Include(s => s.StudentObj)
            .Include(s => s.SpecalityObject)
            .Search(paramters.SearchTerm!,paramters.Level,paramters.Specality)
            .ToListAsync();
        public void CreateSpecality(int specality,string studentId)
        {
           var researcher= FindByCondition(r=>r.StudentId==studentId,trackChanges:true).FirstOrDefault();
            researcher!.SpecalityId= specality;
        }
        public void CreatePapersToResearcher(Guid researcherId, List<Paper> papers)
        {
            var researcher =  FindByCondition(s => s.Id == researcherId, trackChanges: true)
                .Include(p => p.Papers)
                .FirstOrDefault();
            foreach (var paper in papers)
            {
                researcher!.Papers.Add(paper);
            }
            _context.SaveChanges();
            int points = researcher!.Papers.Count / 2;
            researcher.Points =1+ points;
            researcher.Level = researcher.Points switch
            {
                1 or 2 or 3 => Level.Beginner,
                4 or 5 or 6 => Level.Intermediate,
                7 or 8 => Level.Professional,
                _ => Level.Expert // this is the default case
            };
           
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
        public async Task<IEnumerable<SingleResearcherDto>> GetAllTaskParticipants(Guid taskId)
             => await FindByCondition(i => i.Tasks.FirstOrDefault(i => i.Id == taskId)!.Id == taskId, trackChanges: false)
             .ProjectTo<SingleResearcherDto>(_mapper.ConfigurationProvider)
             .ToListAsync();
        public async Task<string?> GetResearcherByStudentId(string studentId)
            => await FindByCondition(r => r.StudentId == studentId,trackChanges:false)
            .Select(r => r.Id.ToString()).FirstOrDefaultAsync();
        public async Task<ICollection<Skill>> GetSkillsAsync()
            => await _context.Skills.AsNoTracking().ToListAsync();
        public async Task<ICollection<Skill>> GetSkillsToStudent()
        {
            var quizzes = await _context.FinalQuizzes.AsNoTracking().ToListAsync();
            var skillsWithQuizzes = new List<Skill>();
            foreach (var quiz in quizzes)
            {
                var skill = await _context.Skills.AsNoTracking().FirstOrDefaultAsync(s => s.Id == quiz.SkillId);
                if(skill is not null && !skillsWithQuizzes.Contains(skill))
                {
                    skillsWithQuizzes.Add(skill);
                }
            }
            return skillsWithQuizzes;
        }
        public async Task<ICollection<Specality>> GetSpecalitiesAsync()
            => await _context.Specalities.AsNoTracking().ToListAsync();
        public async Task<ICollection<Topic>> GetTopicsAsync()
            => await _context.Topics.AsNoTracking().ToListAsync();
        public void CreateSpecialResearcher(Researcher researcher,string studentId)
        {

            var student = _context.Students.AsNoTracking().Where(s => s.Id == studentId).FirstOrDefault();
            if (student is not null)
            {
                researcher.StudentId = studentId;
                Create(researcher);
                DetermineLevel(researcher.Id);
            }
        }
        private void DetermineLevel(Guid sresearcherId)
        {
            var researcher = _context.Researchers.FirstOrDefault(r => r.Id == sresearcherId);
            if (researcher is not null)
            {
                var pointscheck = researcher!.Points + 1;
                researcher.Level = pointscheck switch
                {
                    1 or 2 or 3 => Level.Beginner,
                    4 or 5 or 6 => Level.Intermediate,
                    7 or 8 => Level.Professional,
                    _ => Level.Expert // this is the default case
                };
                researcher!.Points++;
            }
        }
    }
}
    
 
