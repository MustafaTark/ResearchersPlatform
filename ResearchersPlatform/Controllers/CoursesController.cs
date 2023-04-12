using AutoMapper;
using Microsoft.AspNetCore.Cors;
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
    [EnableCors("_myAllowSpecificOrigins")]
    public class CoursesController : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private IFilesRepository _filesRepository;
        //private readonly ILogger _logger;

        public CoursesController(
            IRepositoryManager repository, IMapper mapper, IFilesRepository filesRepository)
        {
            _repositoryManager = repository;
            _mapper = mapper;
            _filesRepository = filesRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var course = await _repositoryManager.Course.GetAllCoursesAsync();
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
            _repositoryManager.Course.CreateCourse(courseEntity);
            await _repositoryManager.SaveChangesAsync();
           
            return Ok(
                new {CourseId= courseEntity.Id }
                );
        }
        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetCourse(Guid courseId)
        {
            if (courseId.ToString().IsNullOrEmpty())
            {
                return BadRequest("Course ID field shouldn't be null or empty");
            }
            var course = await _repositoryManager.Course.GetCourseByIdAsync(courseId, trackChanges: false);
            if (course is null)
            {
                return NotFound($"Course with ID: {courseId} doesn't exist in the database ");
            }
            var courseDto = _mapper.Map<CourseDto>(course);
            return Ok(courseDto);
        }
        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteCourse(Guid courseId)
        {
            if(courseId.ToString().IsNullOrEmpty())
            {
                return BadRequest("Course ID field shouldn't be null or empty");
            }
            var course = await _repositoryManager.Course.GetCourseByIdAsync(courseId, trackChanges: false);
            if (course is null)
            {
                return NotFound($"Course with ID: {courseId} doesn't exist in the database ");
            }
            _repositoryManager.Course.DeleteCourse(course);
            await _repositoryManager.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("{courseId}")]
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
            var courseEntity = await _repositoryManager.Course.GetCourseByIdAsync(courseId, trackChanges: true);
            if (courseEntity is null)
            {
                return NotFound($"Course with ID: {courseId} doesn't exist in the database ");
            }
            _mapper.Map(course, courseEntity);
            await _repositoryManager.SaveChangesAsync();
            return NoContent();

        }
        [HttpGet("Students")]
        public async Task<IActionResult> GetAllStudentsEnrolledInCourse(Guid courseId)
        {
            if (courseId.ToString().IsNullOrEmpty())
            {
                return BadRequest("Course ID field shouldn't be null or empty");
            }
            var course = await _repositoryManager.Course.GetCourseByIdAsync(courseId, trackChanges: false);
            if (course is null)
            {
                return NotFound($"Course with ID: {courseId} doesn't exist in the database ");
            }
            var student = await _repositoryManager.Student.GetAllStudentsEnrolledInCourseAsync(courseId, trackChanges: false);
            var studentDto = _mapper.Map<IEnumerable<StudentDto>>(student);
            return Ok(studentDto);
        }
        [HttpPut("Enrollment")]
        public async Task<IActionResult> EnrollForACourse([FromBody] EnrollmentDto enrollment ,Guid courseId)
        {
            if(courseId.ToString().IsNullOrEmpty())
            {
                return BadRequest("Course ID field shouldn't be null or empty");
            }
            if (enrollment is null)
            {
                return BadRequest("EnrollmentDto object sent from client is null");
            }
            var student = await _repositoryManager.Student.GetStudentByIdAsync(enrollment.studentId, trackChanges: true);
            var course = await _repositoryManager.Course.GetCourseByIdAsync(courseId, trackChanges: true);

            if (student is null || course is null)
            {
                return BadRequest($"Student's ID (OR) Course's ID doesn't exist in the database");

            }
            var enrolled = await _repositoryManager.Student.CheckToEnroll(courseId, enrollment.studentId);
            if(enrolled == false)
            {
                return BadRequest("This student is already enrolled in the course");
            }
            _repositoryManager.Student.EnrollForCourse(courseId, student);
            _mapper.Map(enrollment,course);
            await _repositoryManager.SaveChangesAsync();
            return NoContent();

        }
        [HttpPost("Sections")]
        public async Task<IActionResult> CreateSectionsToCourse(Guid courseId,
                                                       [FromBody]  List<SectionForCreateDto> sections)
        {
            if (courseId.ToString().IsNullOrEmpty())
            {
                return BadRequest("Course ID field shouldn't be null or empty");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest($"Something Wrong in Model{ModelState}");
            }
            var course=await _repositoryManager.Course.GetCourseByIdAsync(courseId,trackChanges: true);
            if(course is null)
            {
                return NotFound();
            }
            var sectionsEntities = _mapper.Map<List<Section>>(sections);
             _repositoryManager.Section.CreateSectionsToCourse(courseId,sectionsEntities);
            await _repositoryManager.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet("Sections/{sectionId}")]
        public async Task<IActionResult> GetSection(Guid sectionId)
        {
            if (sectionId.ToString().IsNullOrEmpty())
            {
                return BadRequest("Section ID field shouldn't be null or empty");
            }
            var section=await _repositoryManager.Section
                                                .GetSectionByIdAsync(sectionId,trackChanges: false);
            if(section is null)
            {
                return NotFound();
            }
            return Ok(section);
        }
        [HttpPost("Videos/{sectionId}"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadVideos(Guid sectionId,
                                [FromForm]  VideoForCreateDto video)
        {
            var section = await _repositoryManager.Section
                                               .GetSectionByIdAsync(sectionId, trackChanges: false);
            if (section is null)
            {
                return NotFound();
            }
            try {
            if (video.File.Length > 0)
            {
                _filesRepository.UploadVideoToSection(sectionId, video.File, video.Title);
                await _repositoryManager.SaveChangesAsync();
            }
            else
            {
                    return BadRequest();
            }
            } catch(Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
            return NoContent();
        }
        [HttpGet("Videos/{videoId}")]
        [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("video/mp4")]
        //[SwaggerOperation("GetVideoById")]
        //[SwaggerResponse(StatusCodes.Status200OK, "The video file", typeof(FileStreamResult))]
        //[SwaggerResponse(StatusCodes.Status404NotFound, "The video file could not be found")]
        public async Task<IActionResult> GetVideo(int videoId)
        {
            
            var fileStream =await  _filesRepository.GetVideoToSection(videoId);
            return new FileStreamResult(fileStream, "video/mp4");
        }
        [HttpGet("Sections/Videos/{sectionId}")]
        public async Task<IActionResult> GetVideosToSection(Guid sectionId)
        {
            var section = await _repositoryManager.Section
                                              .GetSectionByIdAsync(sectionId, trackChanges: false);
            if (section is null)
            {
                return NotFound();
            }
            var videos = await _filesRepository.GetAllVideosToSection(sectionId);
            return Ok(videos);
        }


    }
}
