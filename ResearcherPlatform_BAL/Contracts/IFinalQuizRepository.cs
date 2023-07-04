using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Contracts
{
    public interface IFinalQuizRepository
    {
        Task<bool> IsValidateToFinalQuiz(int skillId, string studentId);
        void CreateQuiz(FinalQuiz quiz);
        void DeleteQuiz(FinalQuiz finalQuiz);
        bool IsValidatedCorrectAnswers(ICollection<AnswerForCreateDto> answers);
        Task<FinalQuizDto> GetSingleFinalQuiz(int skillId,string studentId, bool trackChanges);
        int GetScore(List<Guid> answersIds);
        Task<bool> IsSuccessedInSkill(int skillId, string studentId);
        void Submit (QuizResults results, int skillId);
        Task UpdateTrails(int skillId, string studentId);
        Task<IEnumerable<FinalQuiz>> GetAllQuizzes(int skillId, bool trackChanges);
        Task<FinalQuiz?> GetQuizById(Guid quizId, bool trackChanges);
    }
}
