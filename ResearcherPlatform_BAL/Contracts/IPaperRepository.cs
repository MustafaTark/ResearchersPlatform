using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Contracts
{
    public interface IPaperRepository
    {
        void CreatePaper(Paper paper , Guid researcherId);
        void UpdatePaper(Paper paper);
        void DeletePaper(Paper paper);
        Task<PaperDto?> GetPaperById(Guid paperId , bool trackChanges);
        Task<Paper?> GetPaperByIdForDeletion(Guid paperId , bool trackChanges);
        Task<IEnumerable<PaperDto?>> GetAllPapers(bool trackChanges);
    }
}
