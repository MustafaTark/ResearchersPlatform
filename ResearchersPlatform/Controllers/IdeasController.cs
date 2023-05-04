using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.IdentityModel.Tokens;
using ResearchersPlatform_BAL.Contracts;
using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_BAL.Repositories;
using ResearchersPlatform_DAL.Models;

namespace ResearchersPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
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
            var researcherIdeasNumber = await _repository.Idea.CheckResearcherIdeasNumber(researcherId);
            if(!researcherIdeasNumber)
            {
                return BadRequest($"You have exceeded Your Limit For Idea Initialization .");
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
        [HttpPost("Requests/SendRequest/{researcherId}/{ideaId}")]
        public async Task<IActionResult> SendRequest(Guid researcherId, Guid ideaId)
        {
            var researcher = await _repository.Researcher.GetResearcherByIdAsync(researcherId,trackChanges:true);
            if(researcher is null)
            {
                return NotFound($"Researcher with ID {researcherId} does not exist in the database");
            }
            var idea = await _repository.Idea.GetIdeaAsync(ideaId, trackChanges:false);
            if(idea is null)
            {
                return NotFound($"Idea with Id {ideaId} doesn't exist in the database");
            }
            var researcherValidation = await _repository.Request.ValidateResearcher(researcherId, ideaId);
            if (!researcherValidation)
                return BadRequest("You are the creator of the Idea, you idiot!");
            var requestValidation=  _repository.Request.ValidateRequest(researcherId,ideaId);
            if (requestValidation)
                return BadRequest("Researcher has already send a request to the Idea");
            var ideaValidation = await _repository.Request.ValidateIdea(ideaId);
            if(!ideaValidation)
                return BadRequest($"Idea with ID {ideaId} is full of participants !");
            var validateResearcher = await _repository.Idea.ValidateResearcherForIdea(ideaId, researcherId);
            if (validateResearcher)
            {
                return BadRequest($"Researcher with ID {researcherId} has already joined to the idea");
            }
            await _repository.Request.SendRequest(researcherId,ideaId);
            await _repository.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost("Requests/{requestId}/{researcherId}")]
        public async Task<IActionResult> AcceptRequest(Guid requestId , Guid researcherId)
        {
            var request = await _repository.Request.GetRequestById(requestId, trackChanges: false);
            if (request is null)
            {
                return NotFound($"Request with ID {requestId} doesn't exist in the database");
            }
            var researcher = await _repository.Researcher.GetResearcherByIdAsync(researcherId, trackChanges: true);
            if (researcher is null)
            {
                return NotFound($"Researcher with ID {researcherId} doesn't exist in the database");
            }
            await _repository.Request.AcceptRequest(requestId, researcherId);
            await _repository.SaveChangesAsync();
            return StatusCode(201);
        }
        [HttpDelete("Requests/{requestId}")]
        public async Task<IActionResult> RejectRequest(Guid requestId)
        {
            var request = await _repository.Request.GetRequestById(requestId, trackChanges: false);
            if (request is null)
            {
                return NotFound($"Request with ID {requestId} doesn't exist in the database");
            }
            _repository.Request.DeleteRequest(requestId, request.ResearcherId);
            await _repository.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet("Request/{ideaId}")]
        public async Task<IActionResult> GetAllRequestsForIdea(Guid ideaId)
        {
            var idea = await _repository.Idea.GetIdeaAsync(ideaId,trackChanges: false);
            if (idea is null)
            {
                return NotFound($"Idea with ID {ideaId} doesn't exist in the database");
            }
            var requests = await _repository.Request.GetAllRequests(ideaId, trackChanges: false);
            return Ok(requests);
        }
        [HttpGet("Requests/SingleRequest/{requestId}")]
        public async Task<IActionResult> GetRequestById(Guid requestId)
        {
            var request = await _repository.Request.GetRequestById(requestId, trackChanges: false);
            if (request is null)
            {
                return NotFound($"Request with ID {requestId} doesn't exist in the database");
            }
            return Ok(request);
        }
        [HttpPost("Tasks/InitiateTask/{ideaId}/{creatorId}")]
        public async Task<IActionResult> InitiateTask([FromBody] TaskForCreateDto taskDto ,Guid ideaId , Guid creatorId)
        {
            if(taskDto is null)
            {
                return BadRequest("TaskForCreateDto object sent from client is null");
            }
            var idea = await _repository.Idea.GetIdeaAsync (ideaId,trackChanges: false);
            if (idea is null)
            {
                return NotFound($"Idea with ID {ideaId} doesn't exist in the database");
            }
            var researcher = await _repository.Researcher.GetResearcherByIdAsync(creatorId, trackChanges: false);
            if(researcher is null)
            {
                return NotFound($"Researcher with ID {creatorId} doesn't exist in the database");
            }
            if(idea.CreatorId != creatorId)
            {
                return BadRequest("You are not the creator if the idea");
            }
            if(idea.Deadline <= taskDto.Deadline)
            {
                return BadRequest("The deadline for the task shouldn't last longer than the deadline for this idea");
            }
            var ideaParticipantNumber = await _repository.Task.IdeaParticipantNumber(ideaId);
            if(ideaParticipantNumber <= taskDto.ParticipantsNumber)
            {
                return BadRequest("Task Participant number shouldn't be bigger than idea participant number");
            }
            var taskEntity = _mapper.Map<TaskIdea>(taskDto);
            await _repository.Task.CreateTask(ideaId,creatorId, taskEntity);
            await _repository.SaveChangesAsync();
            return StatusCode(201, "Task has been created successfully");
        }
        [HttpGet("Tasks/AllTasks/{ideaId}")]
        public async Task<IActionResult> GetTaksForIdea(Guid ideaId)
        {
            var idea = await _repository.Idea.GetIdeaAsync(ideaId,trackChanges: false);
            if(idea is null)
            {
                return NotFound($"Idea with ID {ideaId} doesn't exist in the database");
            }
            var ideaTasks = await _repository.Task.GetAllTasksForAnIdeaAsync(ideaId,trackChanges: false);
            if(ideaTasks is null)
            {
                return NotFound($"Idea with ID {ideaId} doesn't have any Tasks yet!");
            }
            return Ok(ideaTasks);
        }
        [HttpGet("Tasks/SingleTask/{taskId}")]
        public async Task<IActionResult> GetTaskById(Guid taskId)
        {
            var task = await _repository.Task.GetTaskByIdAsync(taskId , trackChanges: false);
            if (task is null)
            {
                return NotFound($"Task with ID {taskId} doesn't exist in the database");
            }
            return Ok(task);
        }
        [HttpPost("Tasks/Participants/{taskId}")]
        public async Task<IActionResult> AssignParticipantsToTask(Guid taskId , [FromBody] List<Guid> participantsIds)
        {
            if(participantsIds.ToString().IsNullOrEmpty())
            {
                return BadRequest("Participant Ids should not be null or empty");
            }
            var task = await _repository.Task.GetTaskByIdAsync(taskId, trackChanges: true);
            if (task is null)
            {
                return NotFound($"Task with ID {taskId} doesn't exist in the database");
            }
            var Ideaparticipants = await _repository.Researcher.GetAllIdeaParticipants(task.IdeaId);
            if(Ideaparticipants is null)
            {
                return BadRequest($"Idea with ID {task.IdeaId} doesn't have any Participants yet");
            }
            foreach(var id in participantsIds)
            {
                var participant = Ideaparticipants.FirstOrDefault();
                if(participant!.Id != id)
                {
                    return BadRequest($"Researcher with ID {participant.Id} doesn't belong to the Idea");
                }
                var validateParticipants = await _repository.Task.ValidateTaskParticipants(participantsIds, taskId);
                if(validateParticipants)
                    return BadRequest($"Participant with ID {id} is already assigned to the task");
                await _repository.Task.AssignParticipantsToTask(taskId,participantsIds);
                await _repository.SaveChangesAsync();
            }
            return NoContent();
        }
    }
}