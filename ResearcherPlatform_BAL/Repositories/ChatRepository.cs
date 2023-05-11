using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ResearchersPlatform_BAL.Contracts;
using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_BAL.ViewModels;
using ResearchersPlatform_DAL.Data;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Repositories
{
    public class ChatRepository : IChatRepository
    {
        public readonly AppDbContext _context;
        public readonly IMapper _mapper;
        public ChatRepository(AppDbContext context , IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<MessageDto>> GetMessagesToIdea(Guid ideaId)
          => await _context.IdeaMessages
                 .Where(i => i.IdeaId == ideaId)
                .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        public async Task<IEnumerable<MessageDto>> GetMessagesToTasks(Guid taskId)
         => await _context.TaskMessages
               .Where(i => i.TaskIdeaId == taskId)
               .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)
               .ToListAsync();

        public void CreateIdeaMessage(Guid ideaId,Guid researcherId,MessageViewModel messageVM)
        {
            var message = new IdeaMessage
            {
                Content = messageVM.Content,
                Date = messageVM.Date,
                ResearcherId = researcherId,
                IdeaId = ideaId,
            };
            _context.IdeaMessages.Add(message);
        } 
        public void CreateTaskMessage(Guid taskId,Guid researcherId,MessageViewModel messageVM)
        {
            var message = new TaskMessage
            {
                Content = messageVM.Content,
                Date = messageVM.Date,
                ResearcherId = researcherId,
                TaskIdeaId = taskId,
            };
            _context.TaskMessages.Add(message);
        }
        //public void CreateTaskMessage(Guid taskId,Guid researcherId,MessageDto messageVM)
        //{
        //    var message = new TaskMessage
        //    {
        //        Content = messageVM.Content,
        //        Date = messageVM.Date,
        //        ResearcherId = researcherId,
        //        TaskIdeaId = taskId,
        //    };
        //    _context.TaskMessages.Add(message);
        //}

    }
}
