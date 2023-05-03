using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Contracts
{
    public interface ITaskRepository
    {
        //void CreateTask(TaskIdea task);
        void UpdateTask(TaskIdea task);
        void DeleteTask(TaskIdea task);
        Task<IEnumerable<TaskDto?>> GetAllTasksForAnIdeaAsync(Guid ideaId, bool trackChanges);
        Task<TaskDto?> GetTaskByIdAsync(Guid taskId, bool trackChanges);
        Task CreateTask(Guid ideaId, Guid creatorId, TaskIdea task);
        Task AssignParticipantsToTask(Guid ideaId , List<Guid> participantsIds);
        Task<int> IdeaParticipantNumber(Guid ideaId);
        Task<bool> ValidateTaskParticipantsBool(List<Guid> participantsIds, Guid taskId);
        //Task ValidateTaskParticipantsBool(List<Guid> participantsIds, Guid taskId);

    }
}
