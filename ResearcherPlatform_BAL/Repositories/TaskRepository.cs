using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ResearchersPlatform_BAL.Contracts;
using ResearchersPlatform_BAL.DTO;
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
        private readonly IMapper _mapper;
        public TaskRepository(AppDbContext context , IMapper mapper):base(context)
        {
            _mapper = mapper;
        }
        public void UpdateTask(TaskIdea task) => Update(task);
        public void DeleteTask(TaskIdea task) => Delete(task);
        public async Task<IEnumerable<TaskDto?>> GetAllTasksForAnIdeaAsync(Guid ideaId, bool trackChanges)
            => await FindByCondition(t => t.IdeaId == ideaId, trackChanges)
            .ProjectTo<TaskDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        public async Task<TaskDto?> GetTaskByIdAsync(Guid taskId, bool trackChanges)
            => await FindByCondition(t => t.Id == taskId, trackChanges)
            .ProjectTo<TaskDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        public async Task<TaskIdea?> GetSingleTaskByIdAsync(Guid taskId, bool trackChanges)
            => await FindByCondition(t => t.Id == taskId, trackChanges)
            .FirstOrDefaultAsync();
        private async Task<bool> ValidateIdeaAndCreator(Guid ideaId, Guid creatorId)
        {
            var idea = await _context.Ideas.FirstOrDefaultAsync(i => i.Id == ideaId && i.CreatorId == creatorId);
            if (idea is not null)
                return true;
            return false;
        }
        public async Task CreateTask(Guid ideaId, Guid creatorId, TaskIdea task)
        {
            if(await ValidateIdeaAndCreator(ideaId,creatorId))
            {
                var idea = await _context.Ideas.FirstOrDefaultAsync(i => i.Id == ideaId);
                    idea!.Tasks.Add(task);
                    task.Progress = 0; // NotStarted
            }
        }
        public async Task AssignParticipantsToTask(Guid taskId,List<Guid> participantsIds)
        {
            var task = await FindByCondition(t => t.Id == taskId, trackChanges: true)
                            .Include(t => t.Participants)
                            .FirstOrDefaultAsync();
            var idea = await _context.Ideas.Include(p=> p.Participants).FirstOrDefaultAsync(i => i.Tasks.FirstOrDefault(t => t.Id == taskId)!.Id == taskId);
            foreach (var id in participantsIds)
            { 
               // await ValidateTaskParticipants(participantsIds, taskId);
                var participant = idea!.Participants.FirstOrDefault(t => t.Id == id);
                    task!.Participants!.Add(participant!);
            }
        }
        public async Task<int> IdeaParticipantNumber(Guid ideaId)
            => await _context.Ideas.Where(i => i.Id == ideaId).Select(i => i.Participants.Count).FirstOrDefaultAsync();
        public async Task<bool> ValidateTask(Guid taskId , Guid ideaId)
        {
            var task = await FindByCondition(t => t.Id == taskId && t.IdeaId == ideaId,trackChanges:false).FirstOrDefaultAsync();
            if(task == null)
                return false;
            return true;
        }
        public async Task<bool> ValidateTaskParticipants(List<Guid> participantsIds, Guid taskId)
        {
            var task = await FindByCondition(p => p.Id == taskId, trackChanges: false).Include(p => p.Participants).FirstOrDefaultAsync();
            foreach (var id in participantsIds)
            {
                var taskParticipant = task!.Participants!.FirstOrDefault(i => i.Id == id);
                if (taskParticipant != null)
                    return false;
            }
            return true;
        }
        public async Task<bool> ValidateTaskSingleParticipant(Guid participantId, Guid taskId)
        {
            var task = await FindByCondition(p => p.Id == taskId,trackChanges: false).Include(t => t.Participants).FirstOrDefaultAsync();
            var participant = task!.Participants!.FirstOrDefault(p => p.Id == participantId);
                if (participant == null)
                    return false;
            return true;
        }
        public void Submit(Guid taskId)
        {
            //var task = FindByCondition(t=>t.Id==taskId, trackChanges: true)
            //    .Include(t=>t.Participants)
            //    .FirstOrDefault();
            //bool isBeforeDeadline = DateTime.Now <= task!.Deadline;
            //if(isBeforeDeadline)
            //{
            //    foreach(var researcher in task.Participants!)
            //    {
            //        researcher.Rate += 100;
            //    }
            //}
            //else if (!isBeforeDeadline)
            //{
            //    foreach (var researcher in task.Participants!)
            //    {
            //        researcher.Rate -= 100;
            //    }
            //}
            //task.Progress = Progress.COMPLETED;
        }
    }
}
