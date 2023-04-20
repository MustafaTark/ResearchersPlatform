using ResearchersPlatform_BAL.ViewModels;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Contracts
{
    public interface IResearcherRepository
    {
        void DeleteResearcher(Researcher researcher);
        Task<ResearcherViewModel?> GetResearcherByIdAsync(Guid researcherId, bool trackChanges);
        Task<IEnumerable<Researcher?>> GetAllResearchersAsync(bool trackChanges);
    }
}
