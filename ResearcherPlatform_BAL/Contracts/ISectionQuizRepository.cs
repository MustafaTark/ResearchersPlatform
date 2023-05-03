using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Contracts
{
    public interface ISectionQuizRepository
    {
        
        void CreateQuiz(SectionQuiz sectionQuiz);
        bool IsValidatedCorrectAnswers(ICollection<AnswerForCreateDto> answers);
        Task<SectionQuizDto> GetSingleQuizAsync(Guid sectionId, bool trackChanges);
        int GetScore(List<Guid> answersIds);
        void Submit(QuizResults result);
    }
}
