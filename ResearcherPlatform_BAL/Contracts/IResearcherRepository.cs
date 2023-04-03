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
        void UpdateResearcher(Researcher researcher);
        void DeleteResearcher(Researcher researcher);
        Task<Researcher?> GetResearcherByIdAsync(string researcherId, bool trackChanges);
        Task<IEnumerable<Researcher?>> GetAllResearchersAsync(bool trackChanges);
        //void UploadImage(IFormFile file, string studentId);
        //FileStream GetImageForStudent(string studentId);
    }
}
