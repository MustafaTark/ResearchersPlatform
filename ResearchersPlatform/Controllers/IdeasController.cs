using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ResearchersPlatform_BAL.Contracts;
using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_BAL.Repositories;
using ResearchersPlatform_DAL.Models;

namespace ResearchersPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdeasController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        //private readonly ILogger _logger;
        private readonly IMapper _mapper;
        public IdeasController(IRepositoryManager repository , IMapper mapper)
        {
            _repository= repository;
            _mapper= mapper;
        }
        [HttpPost("InitiateIdea/{researcherId}")]
        public async Task<IActionResult> InitiateIdea([FromBody] IdeaForCreateDTO ideaDto, Guid researcherId)
        {
            if(ideaDto == null)
            {
                return BadRequest("IdeaForCreateDTO object sent from client is null");
            }
            var researcher = await _repository.Researcher.GetResearcherByIdAsync(researcherId,trackChanges:true);
            if (researcher == null)
            {
                return NotFound($"Researcher With ID {researcherId} doesn't exist in the database");
            }
            var ideaValidation = await _repository.Idea.ValidateIdeaCreation(researcherId);
            if(ideaValidation)
            {
                var availableTopics = await _repository.Idea.GetAvailableTopics(researcherId);
                if (availableTopics.Count() == 0)
                {
                    return BadRequest("No available topics for the researcher.");
                }
                if (!availableTopics.Any(t => t.Id == ideaDto.TopicId))
                {
                    return BadRequest("Invalid topic selected for the idea.");
                }
                var ideaEntity = _mapper.Map<Idea>(ideaDto);
                ideaEntity.CreatorId = researcherId;
                ideaEntity.ParticipantsNumber = 1;
                _repository.Idea.CreateIdea(ideaEntity);
                await _repository.SaveChangesAsync();
                return StatusCode(201, new { ideaId = ideaEntity.Id });
            }
            else
            {
                return BadRequest("Researcher is not validated or does not have enough points to initiate an idea.");
            }
        }
        [HttpGet("SingleIdea/{ideaId}")]
        public async Task<IActionResult> GetIdeaById(Guid ideaId)
        {
            var idea = await _repository.Idea.GetIdeaAsync(ideaId, trackChanges: false);
            if(idea is null)
            {
                return NotFound($"Idea with ID {ideaId} doesn't exist in the database");
            }
            var ideaEntity = _mapper.Map<IdeaDto>(idea);
            return Ok(ideaEntity);
        }
        [HttpGet("ResearcherIdeas/{researcherId}")]
        public async Task<IActionResult> GetIdeasForResearcher(Guid researcherId)
        {
            var researcher = await _repository.Researcher.GetResearcherByIdAsync(researcherId, trackChanges: false);
            if (researcher is null)
            {
                return NotFound($"Researcher with ID {researcherId} doesn't exist in the database");
            }
            var ideas = await _repository.Idea.GetAllIdeasForCreatorAsync(researcherId, trackChanges: false);
            if (ideas is null)
            {
                return NotFound($"Researcher with ID {researcherId} doesn't have any Ideas in the database");
            }
            var IdeaEntities = _mapper.Map<IEnumerable<IdeaDto>>(ideas);
            return Ok(IdeaEntities);
        }
        [HttpDelete("{ideaId}")]
        public async Task<IActionResult> DeleteIdea(Guid ideaId)
        {
            var idea = await _repository.Idea.GetIdeaAsync(ideaId, trackChanges: false);
            if(idea is null)
            {
                return NotFound($"Idea with ID {ideaId} doesn't exist in the database");
            }
            _repository.Idea.DeleteIdea(idea);
            await _repository.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost("Invitations/{ideaId}/{creatorId}")]
        public async Task<IActionResult> SendInvitation([FromForm] InvitationForCreationDto invitationDto
            ,Guid ideaId , Guid creatorId)
        {
            if (invitationDto == null)
            {
                return BadRequest("InvitationForCreationDto object sent from client is null");
            }
            var idea = await _repository.Idea.GetIdeaAsync(ideaId, trackChanges: true);
            if (idea is null)
            {
                return NotFound($"Idea with ID {ideaId} doesn't exist in the database");
            }
            var researcher = await _repository.Researcher.GetResearcherByIdAsync(creatorId, trackChanges: true);
            if(researcher is null)
            {
                return NotFound($"researcher with ID {researcher} doesn;t exist in the database");
            }
            var invitationEntity = _mapper.Map<Invitation>(invitationDto);
            _repository.Invitation.CreateInvitation(ideaId , creatorId , invitationEntity);
            await _repository.SaveChangesAsync();
            return StatusCode(201, new { invitationId = invitationEntity.Id });

        }
    }
}
