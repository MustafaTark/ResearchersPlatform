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
