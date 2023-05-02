using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ResearchersPlatform.Hubs;
using ResearchersPlatform_BAL.Contracts;
using ResearchersPlatform_BAL.ViewModels;

namespace ResearchersPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    public class ChatController : ControllerBase
    {
        private IHubContext<DiscussionHub, IChatClient> _discussionHub;
        private IHubContext<ChatHub, IChatClient> _chatHub;
        private readonly IRepositoryManager _repositoryManager;
        public ChatController(IHubContext<DiscussionHub, IChatClient> discussionHub, IHubContext<ChatHub, IChatClient> chatHub, IRepositoryManager repositoryManager)
        {
            _discussionHub = discussionHub;
            _chatHub = chatHub;
            _repositoryManager = repositoryManager;
        }
        [HttpPost("Discussion/{ideaId}")]
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
           await  _discussionHub.Clients.All.ReceiveMessage(message);
            return Ok(message);
           // _discussionHub.Clients.Clients(researcherId.ToString());
        }
    }
}
