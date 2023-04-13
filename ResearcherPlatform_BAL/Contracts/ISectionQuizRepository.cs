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
        Task<SectionQuizDto> GetSingleQuizAsync(Guid sectionId, bool trackChanges);
        void Submit(QuizResults result);
    }
}
