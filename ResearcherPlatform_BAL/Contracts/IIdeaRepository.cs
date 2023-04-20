using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Contracts
{
    public interface IIdeaRepository
    {
        void CreateIdea(Idea idea);
        void UpdateIdea(Idea idea);
        void DeleteIdea(Idea idea);
        Task<IEnumerable<Idea?>> GetAllIdeasAsync(bool trackChanges);
        Task<Idea?> GetIdeaAsync(Guid odeaId , bool trackChanges);
        Task<IEnumerable<Idea?>> GetAllIdeasForResearcherAsync(Guid researcherId , bool trackChanges);
    }
}
