using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.IdentityModel.Tokens;
using ResearchersPlatform_BAL.Contracts;
using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_BAL.Repositories;
using ResearchersPlatform_DAL.Models;
using System.Data;

namespace ResearchersPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    public class IdeasController : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        public IdeasController(IRepositoryManager repository , IMapper mapper)
        {
            _repositoryManager= repository;
            _mapper= mapper;
        }
        [HttpGet]
        [Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> GetAllIdeas()
        {
            var ideas = await _repositoryManager.Idea.GetAllIdeas(trackChanges: false);
            if (ideas is null)
            {
                return NotFound("There are no ideas in the database");
            }
            var ideaEntity = _mapper.Map<IEnumerable<IdeaDto>>(ideas);
            return Ok(ideaEntity);
        }
        [HttpPost("InitiateIdea/{researcherId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> InitiateIdea([FromBody] IdeaForCreateDTO ideaDto, Guid researcherId)
        {
            if(ideaDto == null)
            {
                return BadRequest("IdeaForCreateDTO object sent from client is null");
            }
            var researcher = await _repositoryManager.Researcher.GetResearcherByIdAsync(researcherId,trackChanges:true);
            if (researcher == null)
            {
                return NotFound($"Researcher With ID {researcherId} doesn't exist in the database");
            }
            var researcherIdeasNumber = await _repositoryManager.Idea.CheckResearcherIdeasNumber(researcherId);
            if(!researcherIdeasNumber)
            {
                return BadRequest($"You have exceeded Your Limit For Idea Initialization .");
            }
            var ideaValidation = await _repositoryManager.Idea.ValidateIdeaCreation(researcherId);
            if(ideaValidation)
            {
                var availableTopics = await _repositoryManager.Idea.GetAvailableTopics(researcherId);
                if (availableTopics.Count() == 0)
                {
                    return BadRequest("No available topics for the researcher.");
                }
                if (!availableTopics.Any(t => t.Id == ideaDto.TopicId))
                {
                    return BadRequest("Invalid topic selected for the idea.");
                }
                var ideaEntity = _mapper.Map<Idea>(ideaDto);
                _repositoryManager.Idea.CreateIdea(ideaEntity , researcherId);
                await _repositoryManager.SaveChangesAsync();
                return StatusCode(201, new { ideaId = ideaEntity.Id });
            }
            else
            {
                return BadRequest("Researcher is not validated or does not have enough points to initiate an idea.");
            }
        }
        [HttpGet("Invitations/{ideaId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetAllInvitationsForIdea(Guid ideaId)
        {
            var idea = await _repositoryManager.Idea.GetIdeaAsync(ideaId, trackChanges: false);
            if (idea is null)
            {
                return NotFound($"Idea with ID {ideaId} doesn't exist in the database");
            }
            var invitation = await _repositoryManager.Invitation.GetAllInvitationsForIdea(ideaId, trackChanges: false);
            if (invitation is null)
            {
                return NotFound($"Idea with ID {ideaId} doesn't have any invitations");
            }
            var invitationEntity = _mapper.Map<IEnumerable<InvitationDto>>(invitation);
            return Ok(invitationEntity);
        }
        [HttpGet("SingleIdea/{ideaId}")]
        [Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> GetIdeaById(Guid ideaId)
        {
            var idea = await _repositoryManager.Idea.GetIdeaAsync(ideaId, trackChanges: false);
            if(idea is null)
            {
                return NotFound($"Idea with ID {ideaId} doesn't exist in the database");
            }
            var ideaEntity = _mapper.Map<IdeaDto>(idea);
            return Ok(ideaEntity);
        }

        [HttpGet("Participants/{ideaId}")]
        [Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> GetAllIdeaParticipants(Guid ideaId)
        {
            var idea = await _repositoryManager.Idea.GetIdeaAsync(ideaId, trackChanges: false);
            if (idea is null)
            {
                return NotFound($"Idea with ID {ideaId} doesn't have any Ideas in the database");
            }
            var researchers = await _repositoryManager.Researcher.GetAllIdeaParticipants(ideaId);
            if(researchers is null)
            {
                return NotFound("There are no participants yet");
            }
            var researchersEntities = _mapper.Map<IEnumerable<SingleResearcherDto>>(researchers);
            return Ok(researchersEntities);
        }
        [HttpDelete("{ideaId}")]
        [Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> DeleteIdea(Guid ideaId)
        {
            var idea = await _repositoryManager.Idea.GetIdeaAsync(ideaId, trackChanges: false);
            if(idea is null)
            {
                return NotFound($"Idea with ID {ideaId} doesn't exist in the database");
            }
            var hasParticipants = await _repositoryManager.Idea.HasParticipants(ideaId);
            if(hasParticipants)
            {
                return BadRequest($"Idea with ID {ideaId} does have Participants , you cant delete it ");
            }
            _repositoryManager.Idea.DeleteIdea(idea);
            await _repositoryManager.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost("Invitations/SendInvitations/{ideaId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> SendInvitation(Guid ideaId, List<Guid> researcherIds)
        {
            if (researcherIds == null || !researcherIds.Any())
            {
                return BadRequest("At least one researcher ID must be provided");
            }
            var idea = await _repositoryManager.Idea.GetIdeaAsync(ideaId, trackChanges: false);
            if (idea == null)
            {
                return NotFound($"Idea with ID {ideaId} doesn't exist in the database");
            }
            var researchers = await _repositoryManager.Researcher.GetAllResearchersAsync(trackChanges: true);
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
                var validateResearcher = await _repositoryManager.Idea.ValidateResearcherForIdea(ideaId, researcherId);
                if (validateResearcher)
                {
                    return BadRequest($"Researcher with ID {researcherId} has already joined to the idea");
                }
            }
            _repositoryManager.Invitation.CreateInvitations(ideaId, researcherIds);
            await _repositoryManager.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost("AcceptInvitations/{invitationId}/{researcherId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> AcceptInvitation(Guid invitationId,Guid researcherId)
        {
            var invitation = await _repositoryManager.Invitation.GetInvitationById(invitationId, trackChanges: false);
            if(invitation is null)
            {
                return NotFound($"Invitation with ID {invitationId} doesn't exist in the database");
            }
            var researcher = await _repositoryManager.Researcher.GetResearcherByIdAsync(researcherId, trackChanges: true);
            if (researcher is null)
            {
                return NotFound($"Researcher with ID {researcherId} doesn't exist in the database");
            }
            var validateInvitation = await _repositoryManager.Invitation.ValidationInvitation(invitationId, researcherId);
            if (!validateInvitation)
                return BadRequest($"Researcher with ID {researcherId} is not invited!");
            var checkIdeaParticipants = await _repositoryManager.Idea.CheckParticipantsNumber(invitation.IdeaId);
            if(!checkIdeaParticipants)
                return BadRequest($"Idea with ID {invitation.IdeaId} is full of participants !");
            var validateResearcher = await _repositoryManager.Idea.ValidateResearcherForIdea(invitation.IdeaId, researcherId);
            if (validateResearcher)
            {
                return BadRequest($"Researcher with ID {researcherId} has already joined to the idea");
            }
            await _repositoryManager.Invitation.AcceptInvitation(invitationId, researcherId);
            await _repositoryManager.SaveChangesAsync();
            return StatusCode(201, new { ideaId = invitation.IdeaId });
        }
        [HttpDelete("RejectInvitation/{invitationId}/{researcherId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> RejectInvitation(Guid invitationId, Guid researcherId)
        {
            var invitation = await _repositoryManager.Invitation.GetInvitationById(invitationId, trackChanges: false);
            if (invitation is null)
            {
                return NotFound($"Invitation with ID {invitationId} doesn't exist in the database");
            }
            var researcher = await _repositoryManager.Researcher.GetResearcherByIdAsync(researcherId, trackChanges: true);
            if (researcher is null)
            {
                return NotFound($"Researcher with ID {researcherId} doesn't exist in the database");
            }
            var validateInvitation = await _repositoryManager.Invitation.ValidationInvitation(invitationId, researcherId);
            if (!validateInvitation)
                return BadRequest($"Researcher with ID {researcherId} is not invited!");

             _repositoryManager.Invitation.DeleteInvitation(invitationId, researcherId);
            await _repositoryManager.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost("Requests/SendRequest/{researcherId}/{ideaId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> SendRequest(Guid researcherId, Guid ideaId)
        {
            var researcher = await _repositoryManager.Researcher.GetResearcherByIdAsync(researcherId,trackChanges:true);
            if(researcher is null)
            {
                return NotFound($"Researcher with ID {researcherId} does not exist in the database");
            }
            var idea = await _repositoryManager.Idea.GetIdeaAsync(ideaId, trackChanges:false);
            if(idea is null)
            {
                return NotFound($"Idea with Id {ideaId} doesn't exist in the database");
            }
            var researcherValidation = await _repositoryManager.Request.ValidateResearcher(researcherId, ideaId);
            if (!researcherValidation)
                return BadRequest("You are the creator of the Idea, you idiot!");
            var requestValidation=  _repositoryManager.Request.ValidateRequest(researcherId,ideaId);
            if (requestValidation)
                return BadRequest("Researcher has already send a request to the Idea");
            var ideaValidation = await _repositoryManager.Request.ValidateIdea(ideaId);
            if(!ideaValidation)
                return BadRequest($"Idea with ID {ideaId} is full of participants !");
            var validateResearcher = await _repositoryManager.Idea.ValidateResearcherForIdea(ideaId, researcherId);
            if (validateResearcher)
            {
                return BadRequest($"Researcher with ID {researcherId} has already joined to the idea");
            }
            await _repositoryManager.Request.SendRequest(researcherId,ideaId);
            await _repositoryManager.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost("Requests/AcceptRequest/{requestId}/{researcherId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> AcceptRequest(Guid requestId , Guid researcherId)
        {
            var request = await _repositoryManager.Request.GetRequestById(requestId, trackChanges: false);
            if (request is null)
            {
                return NotFound($"Request with ID {requestId} doesn't exist in the database");
            }
            var researcher = await _repositoryManager.Researcher.GetResearcherByIdAsync(researcherId, trackChanges: true);
            if (researcher is null)
            {
                return NotFound($"Researcher with ID {researcherId} doesn't exist in the database");
            }
            await _repositoryManager.Request.AcceptRequest(requestId, researcherId);
            await _repositoryManager.SaveChangesAsync();
            return StatusCode(201);
        }
        [HttpDelete("Requests/{requestId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> RejectRequest(Guid requestId)
        {
            var request = await _repositoryManager.Request.GetRequestById(requestId, trackChanges: false);
            if (request is null)
            {
                return NotFound($"Request with ID {requestId} doesn't exist in the database");
            }
            _repositoryManager.Request.DeleteRequest(requestId, request.ResearcherId);
            await _repositoryManager.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet("Requests/{ideaId}")]
        public async Task<IActionResult> GetAllRequestsForIdea(Guid ideaId)
        {
            var idea = await _repositoryManager.Idea.GetIdeaAsync(ideaId,trackChanges: false);
            if (idea is null)
            {
                return NotFound($"Idea with ID {ideaId} doesn't exist in the database");
            }
            var requests = await _repositoryManager.Request.GetAllRequests(ideaId, trackChanges: false);
            return Ok(requests);
        }
        [HttpGet("Requests/SingleRequest/{requestId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetRequestById(Guid requestId)
        {
            var request = await _repositoryManager.Request.GetRequestById(requestId, trackChanges: false);
            if (request is null)
            {
                return NotFound($"Request with ID {requestId} doesn't exist in the database");
            }
            return Ok(request);
        }
        [HttpPost("Tasks/InitiateTask/{ideaId}/{creatorId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> InitiateTask([FromBody] TaskForCreateDto taskDto ,Guid ideaId , Guid creatorId)
        {
            if(taskDto is null)
            {
                return BadRequest("TaskForCreateDto object sent from client is null");
            }
            var idea = await _repositoryManager.Idea.GetIdeaAsync (ideaId,trackChanges: false);
            if (idea is null)
            {
                return NotFound($"Idea with ID {ideaId} doesn't exist in the database");
            }
            var researcher = await _repositoryManager.Researcher.GetResearcherByIdAsync(creatorId, trackChanges: false);
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
            var ideaParticipantNumber = await _repositoryManager.Task.IdeaParticipantNumber(ideaId);
            if(ideaParticipantNumber <= taskDto.ParticipantsNumber)
            {
                return BadRequest("Task Participant number shouldn't be bigger than idea participant number");
            }
            var taskEntity = _mapper.Map<TaskIdea>(taskDto);
            await _repositoryManager.Task.CreateTask(ideaId,creatorId, taskEntity);
            await _repositoryManager.SaveChangesAsync();
            return StatusCode(201, "Task has been created successfully");
        }
        [HttpGet("Tasks/AllTasks/{ideaId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetTaksForIdea(Guid ideaId)
        {
            var idea = await _repositoryManager.Idea.GetIdeaAsync(ideaId,trackChanges: false);
            if(idea is null)
            {
                return NotFound($"Idea with ID {ideaId} doesn't exist in the database");
            }
            var ideaTasks = await _repositoryManager.Task.GetAllTasksForAnIdeaAsync(ideaId,trackChanges: false);
            if(ideaTasks is null)
            {
                return NotFound($"Idea with ID {ideaId} doesn't have any Tasks yet!");
            }
            return Ok(ideaTasks);
        }
        [HttpGet("Tasks/SingleTask/{taskId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetTaskById(Guid taskId)
        {
            var task = await _repositoryManager.Task.GetTaskByIdAsync(taskId , trackChanges: false);
            if (task is null)
            {
                return NotFound($"Task with ID {taskId} doesn't exist in the database");
            }
            return Ok(task);
        }
        [HttpPost("Tasks/Participants/{taskId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> AssignParticipantsToTask(Guid taskId , [FromBody] List<Guid> participantsIds)
        {
            if(!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            var task = await _repositoryManager.Task.GetTaskByIdAsync(taskId, trackChanges: true);
            if (task is null)
            {
                return NotFound($"Task with ID {taskId} doesn't exist in the database");
            }
            if(task.ParticipantsNumber < participantsIds.Count)
            {
                return BadRequest($"Allowd Participants count is {task.ParticipantsNumber}");
            }
            var validateParticipants = await _repositoryManager.Task.ValidateTaskParticipants(participantsIds, taskId);
            if(!validateParticipants)
                return BadRequest($"one of the Participant is already assigned to the task");
            await _repositoryManager.Task.AssignParticipantsToTask(taskId,participantsIds);
            await _repositoryManager.SaveChangesAsync();
            return NoContent();
        }
    }
}