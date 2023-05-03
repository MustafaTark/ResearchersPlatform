using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ResearchersPlatform_BAL.Contracts;
using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_DAL.Data;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Repositories
{
    public class SectionQuizRepository : GenericRepository<SectionQuiz>, ISectionQuizRepository
    {
        private readonly IMapper _mapper;

        public SectionQuizRepository(AppDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }
        public bool IsValidatedCorrectAnswers(ICollection<AnswerForCreateDto> answers)
        {
            int correctAnswers = 0;
            foreach (var answer in answers)
            {
                if (answer.IsCorrectAnswer)
                    correctAnswers++;
            }
            if (correctAnswers > 1)
                return false;
            return true;
        }
        public void CreateQuiz(SectionQuiz sectionQuiz)
          =>Create(sectionQuiz);

        public async Task<SectionQuizDto> GetSingleQuizAsync(Guid sectionId,bool trackChanges)
        {
          var quizes= await FindByCondition(q=>q.SectionId==sectionId,trackChanges)
                .Include(s=>s.Questions)
                .ThenInclude(a=>a.Answers)
                .ProjectTo<SectionQuizDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            Random random = new();
            int randomIndex = random.Next(quizes.Count);
            return quizes[randomIndex];

        }
        public int GetScore(List<Guid> answersIds)
        {
            int score = 0;
            foreach (var answerId in answersIds)
            {
                var answer = _context.Answers.AsNoTracking()
                     .Where(a => a.Id == answerId)
                     .Select(a => new { a.IsCorrectAnswer, a.QuestionId })
                     .FirstOrDefault();
                int questionScore = _context.Questions.AsNoTracking()
                    .Where(q => q.Id == answer!.QuestionId)
                    .Select(q => q.Score)
                    .FirstOrDefault();
                if (answer!.IsCorrectAnswer)
                    score += questionScore;

            }
            return score;
        }
        public void Submit(QuizResults results)
        {
            var quiz = _context.SectionQuizzes
                .Where(q => q.Id == results.QuizId)
                .Select(s=>new {s.MaxScore })
                .FirstOrDefault();

            if (results.Score >= quiz!.MaxScore!)
            {
                results.IsSuccessed = true;
            }
            _context.QuizResults.Add(results);
        }
    }
}
