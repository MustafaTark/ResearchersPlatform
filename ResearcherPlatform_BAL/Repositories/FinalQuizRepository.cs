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
using static System.Collections.Specialized.BitVector32;

namespace ResearchersPlatform_BAL.Repositories
{
    public class FinalQuizRepository : GenericRepository<FinalQuiz>, IFinalQuizRepository
    {
        private readonly IMapper _mapper;
        public FinalQuizRepository(AppDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public void CreateQuiz(FinalQuiz quiz)
           => Create(quiz);
        public async Task<bool> IsValidateToFinalQuiz(int skillId, string studentId)
        {
            var course = await _context.Courses
                                             .Where(c => c.SkillId == skillId
                                                  &&
                                                 c.Students
                                                 .FirstOrDefault(s => s.Id == studentId)!
                                                 .Id == studentId)
                                            .FirstOrDefaultAsync();
            if (!(course == null))
            {
                return true;
               
            }
            else
            {
                var studentTrails = await _context.StudentQuizTrails.Where(t => t.StudentId == studentId
                                                                      && t.SkillId == skillId)
                   .Select(t => t.Trails).FirstOrDefaultAsync();
                if (studentTrails > 0)
                    return true;
                return false;
            }
        }

        public async Task<FinalQuizDto> GetSingleFinalQuiz(int skillId,string studentId,bool trackChanges)
        {
          
            var quizes = await FindByCondition(q => q.SkillId == skillId, trackChanges)
                 .Include(s => s.Questions)
                 .ThenInclude(a => a.Answers)
                 .ProjectTo<FinalQuizDto>(_mapper.ConfigurationProvider)
                 .ToListAsync();
            Random random = new();
            int randomIndex = random.Next(quizes.Count);
            return quizes[randomIndex];
        }

        public void Submit(QuizResults results,int skillId)
        {
            var quiz = _context.FinalQuizzes
                 .Where(q => q.Id == results.QuizId)
                 .Select(s => new { s.MaxScore })
                 .FirstOrDefault();

            if (results.Score >= quiz!.MaxScore!)
            {
                results.IsSuccessed = true;
                CreateBadge(skillId,results.StudentId);
                InitiateResearcher(results.StudentId);
            }
            _context.QuizResults.Add(results);
            
        }
        public async Task UpdateTrails(int skillId , string studentId)
        {
            var course = _context.Courses
                                            .Where(c => c.SkillId == skillId
                                                 &&
                                                c.Students
                                                .FirstOrDefault(s => s.Id == studentId)!
                                                .Id == studentId)
                                           .FirstOrDefault();
            if (course == null)
            {
                var studenTrails =await _context.StudentQuizTrails
                                           .Where(s => s.StudentId == studentId
                                                                      && s.SkillId == skillId)
                                          .ExecuteUpdateAsync(t => t.SetProperty(s => s.Trails,
                                                                                    s => s.Trails - 1));
            }
        }
        private void CreateBadge(int skillId, string studentId)
        {
            var skillName= _context.Skills.Where(s=>s.Id==skillId).Select(s => s.Name).FirstOrDefault();
            var badge = new Badge
            {
                Name=skillName!,
                IsVisible=true,
            };
            var student= _context.Students.FirstOrDefault(s=>s.Id == studentId)!;
            student.Badges.Add(badge);
           // _context.SaveChanges();
        }
        private void InitiateResearcher(string studentId)
        {
            var researcher = _context.Researchers.AsNoTracking().Where(s => s.StudentId == studentId).FirstOrDefault();
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
            else
            {
                var student = _context.Students.AsNoTracking().FirstOrDefault(s => s.Id == studentId);
                var researcherEntity = new Researcher
                {
                    StudentId = studentId,
                    Level = Level.Beginner,
                    SpecalityId = 1,
                };
                researcherEntity.Points++;
                _context.Researchers.Add(researcherEntity!);
            }
           // _context.SaveChanges();
        }
    }
}
