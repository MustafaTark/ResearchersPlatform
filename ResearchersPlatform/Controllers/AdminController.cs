using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ResearchersPlatform_BAL.Contracts;
using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_DAL.Models;
using System.Data;

namespace ResearchersPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userAdminManager;
        private readonly IAuthService _authService;
        private IRepositoryManager _repository;
        public AdminController(
            IMapper mapper,
            UserManager<User> userAdminManager,
        IAuthService authService,
        IRepositoryManager repository)
        {
            _mapper = mapper;
            _userAdminManager = userAdminManager;
            _authService = authService;
            _repository = repository;
        }
        [HttpPost("Registration")]
        public async Task<IActionResult> RegisterUser([FromBody] AdminForRegisterDto userForRegistration)
        {
            var user = _mapper.Map<User>(userForRegistration);
            var result = await _userAdminManager.CreateAsync(user, userForRegistration.Password!);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            await _userAdminManager.AddToRoleAsync(user, "Admin");
            return StatusCode(201);
        }
        [HttpPost("Login")]

        public async Task<IActionResult> Authenticate([FromBody] UserForLoginDto user)
        {
            if (!await _authService.ValidateUser(user))
            {
                return Unauthorized("Authentication failed. Wrong user name or password.");
            }
            var admin = await _userAdminManager.FindByNameAsync(user.Email!);
            return Ok(
            new
            {
                Token = await _authService.CreateToken(),
                UserId = await _userAdminManager.GetUserIdAsync(admin!)
            }
            );
        }
        [HttpGet("Researchers")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetResearchersCount()
        {
            var researchersCount = _repository.Admin.ResearchersCount(trackChanges: false);
            if (researchersCount == 0)
            {
                return NotFound("There are no Researchers in the system yet");
            }
            return Ok(researchersCount);
        }
        [HttpGet("Ideas/Count")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetIdeasCount()
        {
            var ideasCount = _repository.Admin.IdeasCount(trackChanges: false);
            if (ideasCount == 0)
            {
                return NotFound("There are no Researchers in the system yet");
            }
            return Ok(ideasCount);
        }
        [HttpPost("Speciality")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddSpeciality([FromBody] SpecialityForCreationDto specialityDto)
        {
            if(specialityDto is null)
            {
                return BadRequest("SpecialityForCreationDto object sent from client is null");
            }
            try { 
            var speciality = _mapper.Map<Specality>(specialityDto);
            _repository.Speciality.CreateSpeciality(speciality);
            await _repository.SaveChangesAsync();
            var specialityToReturn = _mapper.Map<SpecialityDto>(speciality);
            return Ok(specialityToReturn);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2601)
                {
                    return Conflict(new { message = "A Speciality with the same name already exists." });
                }
                else
                {
                    return StatusCode(400, new { message = "An error occurred while creating the topic." });
                }
            }
        }
        [HttpPost("Topic")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddTopic([FromBody] TopicForCreationDto topicDto)
        {
            if (topicDto is null)
            {
                return BadRequest("TopicForCreationDto object sent from client is null");
            }
            try { 
            var topic = _mapper.Map<Topic>(topicDto);
            _repository.Admin.AddTopic(topic);
            await _repository.SaveChangesAsync();
            var topicToReturn = _mapper.Map<TopicsDto>(topic);
            return Ok(topicToReturn);
            }
            catch(DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2601)
                {
                    return Conflict(new { message = "A topic with the same name already exists." });
                }
                else
                {
                    return StatusCode(400, new { message = "An error occurred while creating the topic." });
                }
            }
        }
        [HttpPost("Skill")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddSkill([FromBody] SkillForCreationDto skillDto)
        {
            if (skillDto is null)
            {
                return BadRequest("SkillForCreationDto object sent from client is null");
            }
            try { 
            var skill = _mapper.Map<Skill>(skillDto);
            _repository.Admin.AddSkill(skill);
            await _repository.SaveChangesAsync();
            var skillToReturn = _mapper.Map<Skill>(skill);
            return Ok(skillToReturn);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2601)
                {
                    return Conflict(new { message = "A Skill with the same name already exists." });
                }
                else
                {
                    return StatusCode(400, new { message = "An error occurred while creating the topic." });
                }
            }
        }
        [HttpGet("Skills")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSkills()
        {
            var skills = await _repository.Researcher.GetSkillsAsync();
            return Ok(skills);
        }
        [HttpDelete("ExpertRequests/{requestId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteExpertRequest(Guid requestId)
        {
            var request = await _repository.ExpertRequest.GetRequestById(requestId,false);
            if (request is null)
                return BadRequest($"There is no Request with ID {requestId} in the database");
            var requestEntity = _mapper.Map<ExpertRequest>(request);
            _repository.ExpertRequest.DeleteRequest(requestEntity);
            await _repository.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet("ProblemCategories")]
        public async Task<IActionResult> GetProblemCategories()
        {
            var categories = await _repository.Problem.GetProblemCategories();
            return Ok(categories);
        }


    }
}


