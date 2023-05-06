using ResearchersPlatform_BAL.DTO;
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
        void CreateIdea(Idea idea , Guid creatorId);
        void UpdateIdea(Idea idea);
        void DeleteIdea(Idea idea);
        Task<IEnumerable<Idea?>> GetAllIdeasAsync(bool trackChanges);
        Task<Idea?> GetIdeaAsync(Guid odeaId , bool trackChanges);
        Task<IEnumerable<Idea?>> GetAllIdeasForResearcherAsync(Guid researcherId , bool trackChanges);
        Task<IEnumerable<Idea?>> GetAllIdeasForCreatorAsync(Guid researcherId, bool trackChanges);
        Task<IEnumerable<IdeaDto>> GetAllIdeas(bool trackChanges);
        Task<bool> ValidateIdeaCreation(Guid researcherId);
        Task<IEnumerable<TopicsDto>> GetAvailableTopics(Guid researcherId);
        Task<bool> ValidateResearcherForIdea(Guid ideaId, Guid researcherId);
        Task<bool> HasParticipants(Guid ideaId);
        Task<bool> CheckParticipantsNumber(Guid ideaId);
        Task<bool> CheckResearcherIdeasNumber(Guid researcherId);

    }
}
