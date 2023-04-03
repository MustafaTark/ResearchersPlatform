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
        void CreateTask(TaskIdea task);
        void UpdateTask(TaskIdea task);
        void DeleteTask(TaskIdea task);
        Task<IEnumerable<TaskIdea?>> GetAllTasksForAnIdeaAsync(Guid ideaId, bool trackChanges);
        Task<TaskIdea?> GetTaskByIdAsync(Guid taskId, bool trackChanges);
    }
}
