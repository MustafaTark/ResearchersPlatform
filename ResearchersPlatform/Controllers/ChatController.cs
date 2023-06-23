using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ResearchersPlatform.Hubs;
using ResearchersPlatform_BAL.Contracts;
using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_BAL.ViewModels;
using System.Data;

namespace ResearchersPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<DiscussionHub, IChatClient> _discussionHub;
        private readonly IHubContext<ChatHub, IChatClient> _chatHub;
        private readonly IHubContext<PrivateChatHub, IChatClient> _privateHub;
        private readonly IRepositoryManager _repositoryManager;
        public ChatController(IHubContext<DiscussionHub, IChatClient> discussionHub,
            IHubContext<ChatHub, IChatClient> chatHub,
            IHubContext<PrivateChatHub, IChatClient> privateHub,
            IRepositoryManager repositoryManager)
        {
            _discussionHub = discussionHub;
            _chatHub = chatHub;
            _repositoryManager = repositoryManager;
            _privateHub = privateHub;
        }
        [HttpPost("Discussion/{ideaId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> PostMessageToDiscussion(Guid ideaId,
            Guid researcherId, [FromBody] MessageViewModel message)
        {
            var researcher = await _repositoryManager.Researcher.GetResearcherByIdAsync(researcherId, trackChanges: true);
            if (researcher == null)
            {
                return NotFound($"Researcher With ID {researcherId} doesn't exist in the database");
            }
           bool isPartcipate= await _repositoryManager.Idea.ValidateResearcherForIdea(ideaId, researcherId);
            if(!isPartcipate)
            {
                return BadRequest($"Researcher With ID {researcherId} doesn't exist in this Idea");
            }
           _repositoryManager.Chat.CreateIdeaMessage(ideaId, researcherId, message);
            await _repositoryManager.SaveChangesAsync();
           await  _discussionHub.Clients.All.ReceiveMessage(message);
            return Ok(message);
        } 
        [HttpPost("Task")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> PostMessageToTask(Guid taskId,
            Guid researcherId, [FromBody] MessageViewModel message)
        {
            var researcher = await _repositoryManager.Researcher.GetResearcherByIdAsync(researcherId, trackChanges: true);
            if (researcher == null)
            {
                return NotFound($"Researcher With ID {researcherId} doesn't exist in the database");
            }
            bool isPartcipate = await _repositoryManager.Task.ValidateTaskSingleParticipant(researcherId, taskId);
            if (!isPartcipate)
            {
                return BadRequest($"Researcher With ID {researcherId} doesn't exist in this Idea");
            }
            _repositoryManager.Chat.CreateTaskMessage(taskId, researcherId, message);
            await _repositoryManager.SaveChangesAsync();
            await  _chatHub.Clients.All.ReceiveMessage(message);
            return Ok(message);
        } 
        [HttpPost("Private")]
        [Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> PostPrivateMessage([FromBody] PrivateMessageDto message)
        {
             _repositoryManager.Chat.CreatePrivateMessage(message);
            await _repositoryManager.SaveChangesAsync();
            await  _privateHub.Clients.All.ReceivePrivate(message);
            return Ok(message);
        }
        [HttpGet("Discussion/{ideaId}")]
        [Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> GetMessagesToIdea(Guid ideaId)
        {
           var messages= await _repositoryManager.Chat.GetMessagesToIdea(ideaId);
            return Ok(messages);
        }[
        HttpGet("Task/{ideaId})")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetMessagesToTask(Guid taskId)
        {
           var messages= await _repositoryManager.Chat.GetMessagesToTasks(taskId);
            return Ok(messages);
        }
       [ HttpGet("Private")]
        [Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> GetMessagesToTask(string senderId,string reciverId)
        {
           var messages= await _repositoryManager.Chat.GetPrivateMessages(senderId,reciverId);
            return Ok(messages);
        }
    }
}
