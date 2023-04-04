using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Contracts
{
    public interface IQuizRepository
    {
        void CreateQuestionsToQuiz(Guid quizId, List<Question> questions);
        Task<Quiz> GetQuiz(Guid quizId);
     //   Task<Quiz> GetQuizToCourse(Guid courseId);
        void CreateQuiz(Quiz quiz);
        void DeleteQuiz(Quiz quiz);
    }
}
