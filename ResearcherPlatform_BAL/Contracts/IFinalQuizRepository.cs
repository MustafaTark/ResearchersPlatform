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
        Task<FinalQuizDto> GetSingleFinalQuiz(int skillId,string studentId, bool trackChanges);
        void Submit (QuizResults results, int skillId);
        Task UpdateTrails(int skillId, string studentId);
    }
}
