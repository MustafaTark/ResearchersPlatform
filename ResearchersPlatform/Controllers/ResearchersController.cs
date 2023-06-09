﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ResearchersPlatform_BAL.Contracts;
using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_BAL.Repositories;
using ResearchersPlatform_BAL.RequestFeatures;
using ResearchersPlatform_DAL.Models;
using System.Data;

namespace ResearchersPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    public class ResearchersController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private IFilesRepository _filesRepository;
        public ResearchersController(IRepositoryManager repository , IMapper mapper , IFilesRepository files)
        {
            _repository= repository;
            _mapper= mapper;
            _filesRepository= files;
        }
        [HttpGet]
        [Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> GetAllResearchers([FromQuery]ResearcherParamters paramters)
        {
            var researchers = await _repository.Researcher.GetAllResearchersAsync(paramters,trackChanges: false);
            if(researchers is null)
            {
                return NotFound("There are no researchers in the database");
            }
            var researchersDto = _mapper.Map<IEnumerable<SingleResearcherDto>>(researchers);
            return Ok(researchersDto);
        }
        [HttpGet("ResearcherId/{studentId}")]
        [Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> GetResearcherByStudentId(string studentId)
        {
            var researcherId = await _repository.Researcher.GetResearcherByStudentId(studentId);
            if(researcherId is null)
            {
                return NotFound($"Student with ID {studentId} not exist in the database");
            }
            return Ok(
                new {
                    ResearcherId= researcherId
                });
        }
        [HttpDelete("{researcherId}")]
        [Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> DeleteResearcher(Guid researcherId)
        {
            if (researcherId.ToString().IsNullOrEmpty())
            {
                return BadRequest("Researcher ID is null or empty! ");

            }
            var researcher = await _repository.Researcher.GetResearcherByIdAsync(researcherId, trackChanges: false);
            if (researcher == null)
            {
                return NotFound($"Researcher with ID {researcherId} doesn't exist in the database");
            }
            _repository.Researcher.DeleteResearcher(researcher);
            await _repository.SaveChangesAsync();
            return NoContent();

        }
        [HttpPut("Speciality/{researcherId}/{specialityId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> AssignSpeciality(Guid researcherId,int specialityId)
        {
            var researcher = await _repository.Researcher.GetResearcherByIdAsync(researcherId,trackChanges:true);
            if(researcher == null)
            {
                return NotFound($"Researcher with ID {researcherId} doesn't exist in the database");
            }
            var speciality = await _repository.Speciality.GetSpecialityById(specialityId, trackChanges: true);
            if (speciality == null)
            {
                return NotFound($"Speciality with ID {specialityId} doesn't exist in the database");
            }
            _repository.Researcher.AddSpeciality(researcherId, specialityId);
            await _repository.SaveChangesAsync();
            var response = new
            {   
                status = "Success",
                message = $"Speciality {speciality.Name} has been assigned to Researcher {researcherId}"
            };
            return Ok(response);
        }
        [HttpDelete("Speciality/{researcherId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSpeciality(Guid researcherId)
        {
            var researcher = await _repository.Researcher.GetResearcherByIdAsync(researcherId, trackChanges: false);
            if(researcher is null)
            {
                return NotFound($"Researcher with ID {researcherId} doesn't exist in the database");
            }
            _repository.Researcher.DeleteSpeciality(researcherId);
            await _repository.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost("Papers/{researcherId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> UploadPaper([FromBody] List<PaperForCreationDto> paperDto
            ,Guid researcherId)
        {
            if (paperDto is null)
            {
                return BadRequest("PaperForCreationDto object sent from client is null!");
            }
            var researcher = await _repository.Researcher.GetResearcherByIdAsync(researcherId, trackChanges: true);
            if(researcher is null)
            {
                return NotFound($"Researcher with ID {researcherId} doesn't exist in the database");
            }
            var papers = _mapper.Map<List<Paper>>(paperDto);
            _repository.Researcher.CreatePapersToResearcher(researcherId, papers);
            await _repository.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("Papers/{paperId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> UpdatePaper([FromBody] PaperForCreationDto paperDto, Guid paperId)
        {
            if(paperDto is null)
            {
                return BadRequest("PaperDto object sent from client is null");
            }
            var paper = await _repository.Paper.GetSinglePaperById(paperId, trackChanges: true);
            if(paper is null)
            {
                return NotFound($"Paper With ID {paperId} doesn't exist in the database");
            }
            _mapper.Map(paperDto,paper);
            await _repository.SaveChangesAsync();
            return StatusCode(201, "Paper has been Updated successfully");

        }

        [HttpDelete("Papers/{paperId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> DeletePaper(Guid paperId)
        {
            var paper = await _repository.Paper.GetPaperByIdForDeletion(paperId, trackChanges: false);
            if (paper is null)
            {
                return NotFound($"Paper With ID {paperId} doesn't exist in the database");
            }
            _repository.Paper.DeletePaper(paper);
            await _repository.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet("{researcherId}")]
        [Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> GetSingleResearcher(Guid researcherId)
        {
            var researcher = await _repository.Researcher.GetSingleResearcherByIdAsync(researcherId,trackChanges: false);
            if(researcher is null)
            {
                return NotFound($"Researcher with ID {researcherId} doesn't exist in the database");
            }
            return Ok(researcher);
        }
        [HttpGet("Invitations/{researcherId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetResearcherInvitations(Guid researcherId)
        {
            var researcher = await _repository.Researcher.GetResearcherByIdAsync(researcherId, trackChanges: false);
            if (researcher is null)
            {
                return NotFound($"Researcher with ID {researcherId} doesn't exist in the database");
            }
            var invitation = await _repository.Invitation.GetAllInvitationsForResearcher(researcherId, trackChanges: false);
            if (invitation is null)
            {
                return NotFound("You have no Invitations");
            }
            return Ok(invitation);
        }
        [HttpGet("Requests/{researcherId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetAllRequestsForResearcher(Guid researcherId)
        {
            var researcher = await _repository.Researcher.GetResearcherByIdAsync(researcherId, trackChanges: false);
            if (researcher is null)
            {
                return NotFound($"Researcher with ID {researcherId} doesn't exist in the database");
            }
            var requests = await _repository.Request.GetAllRequestsForResearcher(researcherId, trackChanges: false);
            return Ok(requests);
        }
        [HttpGet("Ideas/{researcherId}")]
        [Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> GetIdeasForResearcher(Guid researcherId)
        {
            var researcher = await _repository.Researcher.GetResearcherByIdAsync(researcherId, trackChanges: false);
            if (researcher is null)
            {
                return NotFound($"Researcher with ID {researcherId} doesn't exist in the database");
            }
            var ideas = await _repository.Idea.GetAllIdeasForResearcherAsync(researcherId, trackChanges: false);
            if (ideas is null)
            {
                return NotFound($"Researcher with ID {researcherId} doesn't have any Ideas in the database");
            }
            var IdeaEntities = _mapper.Map<IEnumerable<IdeaDto>>(ideas);
            return Ok(IdeaEntities);
        }

        [HttpGet("Skills")]
        [Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> GetSkillsForStudent()
        {
            var skills = await _repository.Researcher.GetSkillsToStudent();
            return Ok(skills);
        }
        [HttpGet("Specialties")]
        [Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> GetSpecialties()
        {
            var Specialties = await _repository.Researcher.GetSpecalitiesAsync();
            return Ok(Specialties);
        } 
        [HttpGet("Topics")]
        [Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> GetTopics()
        {
            var topics = await _repository.Researcher.GetTopicsAsync();
            return Ok(topics);
        }
        [HttpPost("Ideas/ExpertRequest")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> SendRequestToExpert([FromBody] ExpertRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest($"Somthing Wrong in Model State {ModelState}");
            }
            var researcher = await _repository.Researcher.GetResearcherByIdAsync(requestDto.ParticipantId,trackChanges:false);
            if (researcher is null)
                return NotFound($"Researcher with ID {requestDto.ParticipantId} doesn't exist in the database");
            var idea = await _repository.Idea.GetIdeaAsync(requestDto.IdeaId, trackChanges: false);
            if (idea is null)
                return NotFound($"Idea with ID {requestDto.IdeaId} doesn't exist in the database");
            var validateParticipant = await _repository.Idea.ValidateResearcherForIdea(requestDto.IdeaId, requestDto.ParticipantId);
            if (!validateParticipant)
                return BadRequest($"Researcher with ID {requestDto.ParticipantId} isn't a participant to the idea");
            var validateParticipantPoints = await _repository.ExpertRequest.ValidateParticipantPoints(requestDto.ParticipantId);
            if (!validateParticipantPoints)
                return BadRequest($"You Should have at least 3 points to send a request");
            var request = _mapper.Map<ExpertRequest>(requestDto);
            _repository.ExpertRequest.CreateRequest(request);
            await _repository.SaveChangesAsync();
            var requestToReturn = _mapper.Map<ExpertRequestDto>(request);
            return Ok(requestToReturn);
        }
        [HttpGet("Ideas/ExpertRequests/{participantId}")]
        //[Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> GetAllExpertRequestsForParticipant(Guid participantId)
        {
            var researcher = await _repository.Researcher.GetResearcherByIdAsync(participantId, false);
            if (researcher is null)
                return BadRequest($"Researcher with ID {participantId} doesn't exist in the database");
            var requests = await _repository.ExpertRequest.GetAllRequestsForResearcher(participantId, trackChanges: false);
            if (requests is null)
            {
                return NotFound("There are no ExpertRequests for this idea");
            }
            var requestEntities = _mapper.Map<IEnumerable<ExpertRequestDto>>(requests);
            return Ok(requestEntities);
        }
    }
}
