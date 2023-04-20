﻿using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        [HttpPost("SectionQuiz/Submit")]
        public async Task<IActionResult> AddSectionQuizResults([FromBody] QuizResultsForCreateDto resultDto)
        {
            var student = await _repositoryManager.Student.GetStudentByIdAsync(resultDto.StudentId!, trackChanges: false);
            if (student is null)
            {
                return BadRequest($"Student with ID: {resultDto.StudentId!} doesn't exist in the database ");
            }
            if (!ModelState.IsValid || resultDto == null)
                return BadRequest($"Something Wrong in Request :{ModelState}");
            var result = _mapper.Map<QuizResults>(resultDto);
            _repositoryManager.SectionQuiz.Submit(result);
            await _repositoryManager.SaveChangesAsync();
            var resultToShow = _mapper.Map<QuizResultsDto>(result);
            return Ok(resultToShow);    
        }
        [HttpPost("FinalQuiz")]
        public async Task<IActionResult> AddFinalQuiz([FromBody] FinalQuizForCreateDto finalQuizDto)
        {
            if (!ModelState.IsValid || finalQuizDto == null)
                return BadRequest($"Something Wrong in Request :{ModelState}");
            var finalQuiz = _mapper.Map<FinalQuiz>(finalQuizDto);
            _repositoryManager.FinalQuiz.CreateQuiz(finalQuiz);
            await _repositoryManager.SaveChangesAsync();
            return NoContent();

        }
        [HttpGet("FinalQuiz/{skillId}")]
        public async Task<IActionResult> GetFinalQuiz(int skillId, string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return BadRequest("Student ID field shouldn't be null or empty");
            }
            var student = await _repositoryManager.Student.GetStudentByIdAsync(studentId!, trackChanges: false);
            if (student is null)
            {
                return BadRequest($"Student with ID: {studentId!} doesn't exist in the database ");
            }
            bool isAvailableQuiz = await _repositoryManager.FinalQuiz.IsValidateToFinalQuiz(skillId,studentId);
            if(!isAvailableQuiz)
            {
                return BadRequest("Not Available");
            }
            var quiz = await _repositoryManager.FinalQuiz
                                                 .GetSingleFinalQuiz(skillId, studentId,trackChanges:false);
            await _repositoryManager.FinalQuiz.UpdateTrails(skillId, studentId);
            return Ok(quiz);
        }
        [HttpPost("FinalQuiz/Submit")]
        public async Task<IActionResult> AddFinalQuizResults(QuizResultsForCreateDto resultDto, int skillId)
        {
            if (skillId.ToString().IsNullOrEmpty())
            {
                return BadRequest("Skill ID field shouldn't be null or empty");
            }
            if (!ModelState.IsValid || resultDto == null)
                return BadRequest($"Something Wrong in Request :{ModelState}");

            var result = _mapper.Map<QuizResults>(resultDto);
            _repositoryManager.FinalQuiz.Submit(result,skillId);
            await _repositoryManager.SaveChangesAsync();
            
            var resultToShow = _mapper.Map<QuizResultsDto>(result);
            return Ok(resultToShow);
        }

    }
}
