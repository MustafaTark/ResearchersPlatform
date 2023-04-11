using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResearchersPlatform_BAL.Contracts;
using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_DAL.Models;
using static System.Collections.Specialized.BitVector32;

namespace ResearchersPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    public class QuizesController : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        public QuizesController(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }
        [HttpPost("SectionQuiz")]
        public async Task<IActionResult> AddSectionQuiz([FromBody]SectionQuizForCreateDto sectionQuizDto)
        {
            if (!ModelState.IsValid || sectionQuizDto == null)
                return BadRequest($"Something Wrong in Request :{ModelState}");
            var sectionQuiz = _mapper.Map<SectionQuiz>(sectionQuizDto);
            _repositoryManager.SectionQuiz.CreateQuiz(sectionQuiz);
            await _repositoryManager.SaveChangesAsync();
            return NoContent();

        }
        [HttpGet("SectionQuiz/{sectionId}")]
        public async Task<IActionResult> GetSectionQuiz(Guid sectionId)
        {
            var section = await _repositoryManager.Section
                                                .GetSectionByIdAsync(sectionId, trackChanges: false);
            if (section is null)
            {
                return NotFound();
            }
            var quizDto = await _repositoryManager.SectionQuiz
                                                  .GetSingleQuizAsync(sectionId, trackChanges: false);
            return Ok(quizDto);
        }
        [HttpPost("Submit")]
        public async Task<IActionResult> AddResults([FromBody] QuizResultsForCreateDto resultDto)
        {
            var student = await _repositoryManager.Student.GetStudentByIdAsync(resultDto.StudentId!, trackChanges: false);
            if (student is null)
            {
                return BadRequest($"Student with ID: {resultDto.StudentId!} doesn't exist in the database ");
            }
            var result = _mapper.Map<QuizResults>(resultDto);
            _repositoryManager.SectionQuiz.Submit(result);
            await _repositoryManager.SaveChangesAsync();
            var resultToShow = _mapper.Map<QuizResultsDto>(result);
            return Ok(resultToShow);    
        }

    }
}
