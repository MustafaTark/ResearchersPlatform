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
            //  TEST 
            if(researcher.Ideas.Count>2)
            {
                return BadRequest($"You have exceeded Your Limit For Idea Initilization which is {researcher.Ideas.Count} .");
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
                _repository.Idea.CreateIdea(ideaEntity , researcherId);
                await _repository.SaveChangesAsync();
                return StatusCode(201, new { ideaId = ideaEntity.Id });
            }
            else
            {
                return BadRequest("Researcher is not validated or does not have enough points to initiate an idea.");
            }
        }
        [HttpGet("Invitations/{ideaId}")]
        public async Task<IActionResult> GetInvitationForIdea(Guid ideaId)
        {
            var idea = await _repository.Idea.GetIdeaAsync(ideaId, trackChanges: false);
            if (idea is null)
            {
                return NotFound($"Idea with ID {ideaId} doesn't exist in the database");
            }
            var invitation = await _repository.Invitation.GetInvitationByIdeaId(ideaId, trackChanges: false);
            if (invitation is null)
            {
                return NotFound($"Idea with ID {ideaId} doesn't have any invitations");
            }
            return Ok(invitation);
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

        [HttpGet("Participants/{ideaId}")]
        public async Task<IActionResult> GetAllIdeaParticipants(Guid ideaId)
        {
            var idea = await _repository.Idea.GetIdeaAsync(ideaId, trackChanges: false);
            if (idea is null)
            {
                return NotFound($"Idea with ID {ideaId} doesn't have any Ideas in the database");
            }
            var researchers = await _repository.Researcher.GetAllIdeaParticipants(ideaId);
            if(researchers is null)
            {
                return NotFound("There are no participants yet");
            }
            var researchersEntities = _mapper.Map<IEnumerable<SingleResearcherDto>>(researchers);
            return Ok(researchersEntities);
        }
        [HttpDelete("{ideaId}")]
        public async Task<IActionResult> DeleteIdea(Guid ideaId)
        {
            var idea = await _repository.Idea.GetIdeaAsync(ideaId, trackChanges: false);
            if(idea is null)
            {
                return NotFound($"Idea with ID {ideaId} doesn't exist in the database");
            }
            var hasParticipants = await _repository.Idea.HasParticipants(ideaId);
            if(hasParticipants)
            {
                return BadRequest($"Idea with ID {ideaId} does have Participants , you cant delete it ");
            }
            _repository.Idea.DeleteIdea(idea);
            await _repository.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost("Invitations/SendInvitations/{ideaId}")]
        public async Task<IActionResult> SendInvitation(Guid ideaId, List<Guid> researcherIds)
        {
            if (researcherIds == null || !researcherIds.Any())
            {
                return BadRequest("At least one researcher ID must be provided");
            }
            var idea = await _repository.Idea.GetIdeaAsync(ideaId, trackChanges: false);
            if (idea == null)
            {
                return NotFound($"Idea with ID {ideaId} doesn't exist in the database");
            }
            var researchers = await _repository.Researcher.GetAllResearchersAsync(trackChanges: true);
            if (researchers == null || !researchers.Any())
            {
                return NotFound($"There are no researchers in the database");
            }
            foreach (var researcherId in researcherIds)
            {
                var researcher = researchers.FirstOrDefault(r => r!.Id == researcherId);
                if (researcher == null)
                {
                    return BadRequest($"Researcher with ID {researcherId} does not exist in the database");
                }
                var validateResearcher = await _repository.Idea.ValidateResearcherForIdea(ideaId, researcherId);
                if (validateResearcher)
                {
                    return BadRequest($"Researcher with ID {researcherId} has already joined to the idea");
                }
            }
            _repository.Invitation.CreateInvitations(ideaId, researcherIds);
            await _repository.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost("Invitations/{invitationId}/{researcherId}")]
        public async Task<IActionResult> AcceptInvitation(Guid invitationId,Guid researcherId)
        {
            var invitation = await _repository.Invitation.GetInvitationById(invitationId, trackChanges: false);
            if(invitation is null)
            {
                return NotFound($"Invitation with ID {invitationId} doesn't exist in the database");
            }
            var researcher = await _repository.Researcher.GetResearcherByIdAsync(researcherId, trackChanges: true);
            if (researcher is null)
            {
                return NotFound($"Researcher with ID {researcherId} doesn't exist in the database");
            }
            var validateInvitation = await _repository.Invitation.ValidationInvitation(invitationId, researcherId);
            if (!validateInvitation)
                return BadRequest($"Researcher with ID {researcherId} is not invited!");
            var checkIdeaParticipants = await _repository.Idea.CheckParticipantsNumber(invitation.IdeaId);
            if(!checkIdeaParticipants)
                return BadRequest($"Idea with ID {invitation.IdeaId} is full of participants !");
            await _repository.Invitation.AcceptInvitation(invitationId, researcherId);
            //await _repository.Invitation.DeleteInvitation(invitationId, researcherId);
            await _repository.SaveChangesAsync();
            return StatusCode(201, new { ideaId = invitation.IdeaId });
        }
        [HttpDelete("Invitations/{invitationId}/{researcherId}")]
        public async Task<IActionResult> RejectInvitation(Guid invitationId, Guid researcherId)
        {
            var invitation = await _repository.Invitation.GetInvitationById(invitationId, trackChanges: false);
            if (invitation is null)
            {
                return NotFound($"Invitation with ID {invitationId} doesn't exist in the database");
            }
            var researcher = await _repository.Researcher.GetResearcherByIdAsync(researcherId, trackChanges: true);
            if (researcher is null)
            {
                return NotFound($"Researcher with ID {researcherId} doesn't exist in the database");
            }
            var validateInvitation = await _repository.Invitation.ValidationInvitation(invitationId, researcherId);
            if (!validateInvitation)
                return BadRequest($"Researcher with ID {researcherId} is not invited!");

             _repository.Invitation.DeleteInvitation(invitationId, researcherId);
            await _repository.SaveChangesAsync();
            return NoContent();
        }

    }
}
