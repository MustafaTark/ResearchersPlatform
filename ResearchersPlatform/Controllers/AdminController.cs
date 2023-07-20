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

        [HttpPost("AdminLogin")]

        public async Task<IActionResult> AuthenticateToAdmin([FromBody] UserForLoginDto user)
        {
            if (!await _authService.ValidateUser(user))
            {
                return Unauthorized();
            }
            var admin = await _userAdminManager.FindByEmailAsync(user.Email!);
            var useradmin = await _userAdminManager.IsInRoleAsync(admin!, "Admin");
            var token = await _authService.CreateToken();
            var userId = await _userAdminManager.GetUserIdAsync(admin!);
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(20),
                Secure = true,
                SameSite = SameSiteMode.Strict
            };
            Response.Cookies.Append("StudentId", userId, cookieOptions);
            Response.Cookies.Append("Token", token, cookieOptions);
            if (!useradmin)
                return NotFound();
            return Ok(
            new
            {
                Token = token,
                UserId = userId
            }
            );
        }
        [HttpPost("Researchers/SpecialAccount/{studentId}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> CreateResearcherSpecialAccounts([FromBody] ResearcherSpecialAccountsForCreationDto researcherDto
            ,string studentId)
        {
            var student = await _repository.Student.GetStudentByIdAsync(studentId,trackChanges:false);
            if(student == null)
                return NotFound($"Student with ID {studentId} doesn't exist in the database");

            if (!ModelState.IsValid || researcherDto == null)
                return BadRequest($"Something Went Wrong in Filling the Form :{ModelState}");
            try { 
            var researcher = _mapper.Map<Researcher>(researcherDto);
            _repository.Researcher.CreateSpecialResearcher(researcher,studentId);
            _repository.Researcher.DetermineLevel(researcher);
            await _repository.SaveChangesAsync();
            return StatusCode(201, new { researcherId = researcher.Id });
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException && sqlException.Number == 2627)
                {
                    return BadRequest("A researcher with the same student ID already exists.");
                }
                return StatusCode(400, new { message = "An error occurred while creating the Researcher." });
            }
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
        [HttpPut("Skills/{skillId}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> UpdateSkill([FromBody] SkillForUpdateDto skillDto,int skillId)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);
            var skill = await _repository.Admin.GetSkillById(skillId);
            if (skill is null)
                return NotFound($"Skill With ID {skillId} doesn't exist in the database");
            _mapper.Map(skillDto, skill);
            await _repository.SaveChangesAsync();
            return StatusCode(201, new { name =  skill.Name });
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
        [HttpGet("ExpertRequests")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllExpertRequests()
        {
            var requests = await _repository.ExpertRequest.GetAllExpertRequests();
            if (requests is null)
                return NotFound("You Have No Expert Requests Yet");
            return Ok(requests);
        }

        [HttpGet("ProblemCategories")]
        public async Task<IActionResult> GetProblemCategories()
        {
            var categories = await _repository.Problem.GetProblemCategories();
            return Ok(categories);
        }
        [HttpGet("Quizzes/{skillId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllQuizzes(int skillId)
        {
            var quizzes = await _repository.FinalQuiz.GetAllQuizzes(skillId,trackChanges:false);
            if (quizzes is null)
                return NotFound("There are no quizzes have been inserted in the database yet");
            var quizzesEntity = _mapper.Map<IEnumerable<FinalQuizDto>>(quizzes);
            return Ok(quizzesEntity);
        }
        [HttpDelete("Quizzes/{quizId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteFinalQuiz(Guid quizId)
        {
            var quiz = await _repository.FinalQuiz.GetQuizById(quizId, trackChanges:false);
            if (quiz is null)
                return BadRequest($"There is no Quiz with ID {quizId} in the database");
            _repository.FinalQuiz.DeleteQuiz(quiz);
            await _repository.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet("Responses/ProbelmCategory/{categoryId}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetAllResponsesByCateogryId(int categoryId)
        {
            var responses = await _repository.Response.GetAllResponsesByCatergoryId(categoryId);
            if (responses is null)
                return NotFound($"There are no responses for Cateogry_ID {categoryId}");
            return Ok(responses);
        }

    }
}


