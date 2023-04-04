using Microsoft.EntityFrameworkCore;
using ResearchersPlatform_BAL.Contracts;
using ResearchersPlatform_DAL.Data;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Repositories
{
    public class QuizRepoistory : GenericRepository<Quiz>, IQuizRepository
    {
        public QuizRepoistory(AppDbContext context):base(context) { }
        public void CreateQuestionsToQuiz(Guid quizId, List<Question> questions)
        {
            throw new NotImplementedException();
        }

        public void CreateQuiz(Quiz quiz)
        {
            throw new NotImplementedException();
        }

        public void DeleteQuiz(Quiz quiz)
        {
            throw new NotImplementedException();
        }

        public Task<Quiz> GetQuiz(Guid quizId)
        {
            throw new NotImplementedException();
        }
    }
}
