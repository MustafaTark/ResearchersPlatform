using Microsoft.EntityFrameworkCore;
using ResearchersPlatform_BAL.Contracts;
using ResearchersPlatform_DAL.Data;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Repositories
{
    public class TaskRepository : GenericRepository<TaskIdea> , ITaskRepository
    {
        public TaskRepository(AppDbContext context):base(context)
        {

        }
        public void CreateTask(TaskIdea task) => Create(task);
        public void UpdateTask(TaskIdea task) => Update(task);
        public void DeleteTask(TaskIdea task) => Delete(task);
        public async Task<IEnumerable<TaskIdea?>> GetAllTasksForAnIdeaAsync(Guid ideaId, bool trackChanges)
            => await FindByCondition(t => t.IdeaId == ideaId, trackChanges)
            .Include(s => s.Progress)
            .ToListAsync();
        public async Task<TaskIdea?> GetTaskByIdAsync(Guid taskId, bool trackChanges)
            => await FindByCondition(t => t.Id == taskId, trackChanges)
            .Include(s => s.Progress)
            .FirstOrDefaultAsync();
    }
}
