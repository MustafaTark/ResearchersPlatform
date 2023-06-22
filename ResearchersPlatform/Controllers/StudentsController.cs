using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ResearchersPlatform_BAL.Contracts;
using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_DAL.Models;
using System.Data;

namespace ResearchersPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    public class StudentsController : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        //private readonly ILogger _logger;

        public StudentsController(
            IRepositoryManager repository,IMapper mapper)
        {
            _repositoryManager = repository;
            _mapper = mapper;
        }
        [HttpGet]
        [Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> GetAllStudents()
        {
            var student = await _repositoryManager.Student.GetAllStudentsAsync();
            if(student is null )
            {
                return NotFound("There are no students in the database");
            }
            var studentDto = _mapper.Map<IEnumerable<StudentDto>>(student);
            return Ok(studentDto);
        }
        [HttpGet("{studentId}")]
        [Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> GetStudent(string studentId)
        {
            if(studentId.IsNullOrEmpty())
            {
                return BadRequest("Student ID field shouldn't be null or empty");
            }
            var student = await _repositoryManager.Student.GetStudentByIdAsync(studentId , trackChanges: false);
            if(student is null )
            {
                return NotFound($"Student with ID: {studentId} doesn't exist in the database ");
            }
            var studentDto = _mapper.Map<StudentDto>(student);
            return Ok(studentDto);
        }
        [HttpPut("studentId")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> UpdateStudent([FromBody] StudentForUpdateDto student,
            string studentId)
        {
            if(student is null)
            {
                return BadRequest("StudentForUpdateDTO object sent from client is null");
            }
            if(studentId.IsNullOrEmpty())
            {
                return BadRequest("Student ID field shouldn't be null or empty");
            }
            var studentEntity = await _repositoryManager.Student.GetStudentByIdAsync(studentId , trackChanges: true);
            if(studentEntity is null)
            {
                return NotFound($"Student with ID: {studentId} doesn't exist in the database ");
            }
            _mapper.Map(student,studentEntity);
            await _repositoryManager.SaveChangesAsync();
            return NoContent();

        }
        [HttpDelete("studentId")]
        [Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> DeleteStudent(string studentId)
        {
            if (studentId.IsNullOrEmpty())
            {
                return BadRequest("Student ID field shouldn't be null or empty");
            }
            var student = await _repositoryManager.Student.GetStudentByIdAsync(studentId , trackChanges: false);
            if (student is null)
            {
                return NotFound($"Student with ID: {studentId} doesn't exist in the database ");
            }
            _repositoryManager.Student.DeleteStudent(student);
            await _repositoryManager.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet("Courses")]
        [Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> GetStudentEnrolledCourses(string studentId)
        {

            if (studentId.IsNullOrEmpty())
            {
                return BadRequest("Student ID field shouldn't be null or empty");
            }
            var student = await _repositoryManager.Student.GetStudentByIdAsync(studentId, trackChanges: false);
            if(student is null)
            {
                return NotFound($"Student with ID: {studentId} doesn't exist in the database ");
            }
            var course = await _repositoryManager.Course.GetAllCoursesForStudentAsync(studentId,trackChanges: false);
            var courseDto = _mapper.Map<IEnumerable<CourseDto>>(course);
            return Ok(courseDto);

        }
        [HttpPost("Problems")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> PostProblem(ProblemFoCreateDto problemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest($"Somthing Wrong in Model State {ModelState}");
            }
            if (problemDto.StudentId.IsNullOrEmpty())
            {
                return BadRequest("Student ID field shouldn't be null or empty");
            }
            if (problemDto.ProblemCategoryId is 0)
            {
                return BadRequest("ProblemCategoryId field shouldn't be null or empty");
            }
            var problem = _mapper.Map<Problem>(problemDto);
            _repositoryManager.Problem.CreateProblem(problem);
            await _repositoryManager.SaveChangesAsync();
            var problemToReturn = _mapper.Map<ProblemDto>(problem);
            return Ok(problemToReturn);
        }
        [HttpGet("Problems")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetProblemsToCategory(int categoryId)
        {
            if (categoryId == 0)
                return BadRequest("categoryId required field");
            var problems = await _repositoryManager.Problem.GetProblemsAsync(categoryId);
            return Ok(problems);
        }
        [HttpGet("Problems/{problemId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetProblemsToCategory(Guid problemId)
        {
            var problem = await _repositoryManager.Problem.GetProblemByIdAsync(problemId);
            if (problem == null)
                return NotFound();
            return Ok(problem);
        }
        [HttpGet("Nationalites")]
        public async Task<IActionResult> GetAllNationalites()
        {
            var nationality = await _repositoryManager.Student.GetAllNationalitiesAsync(trackChanges:false);
            if(nationality == null)
                return NotFound($"There are no Nationalities inserted to the database yet ");
            return Ok(nationality);
        }
        [HttpPost("Responses")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateResponse([FromBody] ResponseForCreationDto responseDto)
        {
            if (responseDto == null)
                return BadRequest("ResponseForCreationDto object sent from client is null");
            var problem = await _repositoryManager.Problem.GetProblemByIdAsync(responseDto.ProblemId);
            if (problem is null)
                return NotFound($"Problem with ID {responseDto.ProblemId} doesn't exist in the database");
            var student = await _repositoryManager.Student.GetStudentByIdAsync(responseDto.StudentId, trackChanges: false);
            if (student is null)
                return NotFound($"Student with ID {responseDto.StudentId} doesn't exist in the database");
            var validateResponse = _repositoryManager.Problem.ValidateResponse(responseDto.StudentId, responseDto.ProblemId);
            if (!validateResponse)
                return BadRequest($"Problem With ID {responseDto.ProblemId} Doesn't belong to this User");
            var response = _mapper.Map<Response>(responseDto);
            _repositoryManager.Response.CreateResponse(response);
            await _repositoryManager.SaveChangesAsync();
            var responseToReturn = _mapper.Map<ResponseDto>(response);
            return Ok(responseToReturn);
        }
        [HttpGet("Responses/{studentId}")]
        [Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> GetResponsesForStudent(string studentId)
        {
            var student = await _repositoryManager.Student.GetStudentByIdAsync(studentId, trackChanges: false);
            if (student is null)
            {
                return NotFound($"Student with ID: {studentId} doesn't exist in the database ");
            }
            var responses = await _repositoryManager.Response.GetAllStudentResponses(trackChanges: false, studentId);
            if (responses is null)
                return NotFound($"Student with ID: {studentId} doesn't have any responses from the Admin yet ");
            return Ok(responses);
        }
        [HttpGet("SingleResponse/{responseId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetResponsesById(Guid responseId)
        {
            var response = await _repositoryManager.Response.GetResponseById(trackChanges: false, responseId);
            if (response is null)
            {
                return NotFound($"Response with ID: {responseId} doesn't exist in the database ");
            }
            return Ok(response);
        }
        [HttpGet("Responses")]
        [Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> GetAllResponses()
        {
            var responses = await _repositoryManager.Response.GetAllResponses(trackChanges: false);
            if (responses is null)
            {
                return NotFound($"There are no Responses are inserted in the database yet");
            }
            return Ok(responses);
        }
        [HttpDelete("responseId")]
        [Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> DeleteResponse(Guid responseId)
        {
            if (responseId.ToString().IsNullOrEmpty())
            {
                return BadRequest("Response ID field shouldn't be null or empty");
            }
            var response = await _repositoryManager.Response.GetSingleResponseById(responseId);
            if (response is null)
            {
                return NotFound($"Response with ID: {responseId} doesn't exist in the database ");
            }
            _repositoryManager.Response.DeleteResponse(response);
            await _repositoryManager.SaveChangesAsync();
            return NoContent();
        }

    }
}
