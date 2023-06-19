using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Contracts
{
    public interface IProblemRepository
    {
        void CreateProblem(Problem problem);
        Task<IEnumerable<ProblemDto>> GetProblemsAsync(int categoryId);
        Task<ProblemDto?> GetProblemByIdAsync(Guid id);
        Task<IEnumerable<ProblemCategory>> GetProblemCategories();
        bool ValidateResponse(string studentId, Guid problemId);
    }
}
