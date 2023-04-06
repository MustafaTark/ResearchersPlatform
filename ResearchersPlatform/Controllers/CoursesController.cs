using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ResearchersPlatform_BAL.Contracts;
using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_DAL.Models;

namespace ResearchersPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        //private readonly ILogger _logger;

        public CoursesController(
            IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var course = await _repository.Course.GetAllCoursesAsync();
            if (course is null)
            {
                return NotFound("There are no courses in the database");
            }
            var courseDto = _mapper.Map<IEnumerable<CourseDto>>(course);
            return Ok(courseDto);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CourseForCreationDto course)
        {
            if(course is null)
            {
                return BadRequest("CourseForCreationDto Object Sent From User os Null");
            }
            var courseEntity = _mapper.Map<Course>(course);
            _repository.Course.CreateCourse(courseEntity);
            await _repository.SaveChangesAsync();
            return StatusCode(201);
        }
        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetCourse(Guid courseId)
        {
            if (courseId.ToString().IsNullOrEmpty())
            {
                return BadRequest("Course ID field shouldn't be null or empty");
            }
            var course = await _repository.Course.GetCourseByIdAsync(courseId, trackChanges: false);
            if (course is null)
            {
                return NotFound($"Course with ID: {courseId} doesn't exist in the database ");
            }
            var courseDto = _mapper.Map<CourseDto>(course);
            return Ok(courseDto);
        }
        [HttpDelete("courseId")]
        public async Task<IActionResult> DeleteCourse(Guid courseId)
        {
            if(courseId.ToString().IsNullOrEmpty())
            {
                return BadRequest("Course ID field shouldn't be null or empty");
            }
            var course = await _repository.Course.GetCourseByIdAsync(courseId, trackChanges: false);
            if (course is null)
            {
                return NotFound($"Course with ID: {courseId} doesn't exist in the database ");
            }
            _repository.Course.DeleteCourse(course);
            await _repository.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("courseId")]
        public async Task<IActionResult> UpdateCourse([FromBody]CourseForUpdateDto course
            ,Guid courseId)
        {
            if (course is null)
            {
                return BadRequest("CourseForUpdateDto object sent from client is null");
            }
            if (courseId.ToString().IsNullOrEmpty())
            {
                return BadRequest("Course ID field shouldn't be null or empty");
            }
            var courseEntity = await _repository.Course.GetCourseByIdAsync(courseId, trackChanges: true);
            if (courseEntity is null)
            {
                return NotFound($"Course with ID: {courseId} doesn't exist in the database ");
            }
            _mapper.Map(course, courseEntity);
            await _repository.SaveChangesAsync();
            return NoContent();

        }
        [HttpGet("Students")]
        public async Task<IActionResult> GetAllStudentsEnrolledInCourse(Guid courseId)
        {
            if (courseId.ToString().IsNullOrEmpty())
            {
                return BadRequest("Course ID field shouldn't be null or empty");
            }
            var course = await _repository.Course.GetCourseByIdAsync(courseId, trackChanges: false);
            if (course is null)
            {
                return NotFound($"Course with ID: {courseId} doesn't exist in the database ");
            }
            var student = await _repository.Student.GetAllStudentsEnrolledInCourseAsync(courseId, trackChanges: false);
            var studentDto = _mapper.Map<IEnumerable<StudentDto>>(student);
            return Ok(studentDto);
        }

    }
}
