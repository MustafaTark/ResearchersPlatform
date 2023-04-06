﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ResearchersPlatform_BAL.Contracts;
using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_DAL.Models;

namespace ResearchersPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        //private readonly ILogger _logger;

        public StudentsController(
            IRepositoryManager repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var student = await _repository.Student.GetAllStudentsAsync();
            if(student is null )
            {
                return NotFound("There are no students in the database");
            }
            var studentDto = _mapper.Map<IEnumerable<StudentDto>>(student);
            return Ok(studentDto);
        }
        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetStudent(string studentId)
        {
            if(studentId.IsNullOrEmpty())
            {
                return BadRequest("Student ID field shouldn't be null or empty");
            }
            var student = await _repository.Student.GetStudentByIdAsync(studentId , trackChanges: false);
            if(student is null )
            {
                return NotFound($"Student with ID: {studentId} doesn't exist in the database ");
            }
            var studentDto = _mapper.Map<StudentDto>(student);
            return Ok(studentDto);
        }
        [HttpPut("studentId")]
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
            var studentEntity = await _repository.Student.GetStudentByIdAsync(studentId , trackChanges: true);
            if(studentEntity is null)
            {
                return NotFound($"Student with ID: {studentId} doesn't exist in the database ");
            }
            _mapper.Map(student,studentEntity);
            await _repository.SaveChangesAsync();
            return NoContent();

        }
        [HttpDelete("studentId")]
        public async Task<IActionResult> DeleteStudent(string studentId)
        {
            if (studentId.IsNullOrEmpty())
            {
                return BadRequest("Student ID field shouldn't be null or empty");
            }
            var student = await _repository.Student.GetStudentByIdAsync(studentId , trackChanges: false);
            if (student is null)
            {
                return NotFound($"Student with ID: {studentId} doesn't exist in the database ");
            }
            _repository.Student.DeleteStudent(student);
            await _repository.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet("Courses")]
        public async Task<IActionResult> GetStudentEnrolledCourses(string studentId)
        {

            if (studentId.IsNullOrEmpty())
            {
                return BadRequest("Student ID field shouldn't be null or empty");
            }
            var student = await _repository.Student.GetStudentByIdAsync(studentId, trackChanges: false);
            if(student is null)
            {
                return NotFound($"Student with ID: {studentId} doesn't exist in the database ");
            }
            var course = await _repository.Course.GetAllCoursesForStudentAsync(studentId,trackChanges: false);
            var courseDto = _mapper.Map<IEnumerable<CourseDto>>(course);
            return Ok(courseDto);

        }

    }
}
