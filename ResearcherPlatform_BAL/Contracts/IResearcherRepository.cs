using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_BAL.RequestFeatures;
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
        Task<ResearcherViewModel?> GetSingleResearcherByIdAsync(Guid researcherId, bool trackChanges);
        Task<Researcher?> GetResearcherByIdAsync(Guid researcherId, bool trackChanges);
        Task<IEnumerable<Researcher?>> GetAllResearchersAsync(ResearcherParamters paramters, bool trackChanges);
        void AddSpeciality(Guid researcherId,int specialityId);
        void DeleteSpeciality(Guid researcherId);
        void CreatePapersToResearcher(Guid researcherId, List<Paper> papers);
        Task<IEnumerable<SingleResearcherDto>> GetAllIdeaParticipants(Guid ideaId);
        Task<IEnumerable<SingleResearcherDto>> GetAllTaskParticipants(Guid taskId);
        Task<string?> GetResearcherByStudentId(string studentId);
        Task<ICollection<Skill>> GetSkillsAsync();
        Task<ICollection<Specality>> GetSpecalitiesAsync();
        Task<ICollection<Topic>> GetTopicsAsync();

    }
}
