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
        Task<IEnumerable<Problem>> GetProblemsAsync(int categoryId);
        Task<ProblemDto?> GetProblemByIdAsync(Guid id);
    }
}
