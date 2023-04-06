using Microsoft.EntityFrameworkCore;
using ResearchersPlatform_BAL.Contracts;
using ResearchersPlatform_DAL.Data;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Repositories
{
    public class StudentRepository : GenericRepository<Student> , IStudentRepository
    {
        private new readonly AppDbContext _context;
        public StudentRepository(AppDbContext dbContext)
            :base(dbContext) 
        {
            _context = dbContext;
        }

        public void UpdateStudent(Student student) => Update(student);
        public void DeleteStudent(Student student) => Update(student);
        public async Task<Student?> GetStudentByIdAsync(string studentId, bool trackChanges)
            => await FindByCondition(e => e.Id == studentId, trackChanges)
            .OrderBy(e => e.UserName)
            .FirstOrDefaultAsync();
        public async Task<IEnumerable<Student?>> GetAllStudentsAsync()
            => await FindAll(trackChanges:false)
            .OrderBy(e => e.UserName)
            .ToListAsync();

        public async Task<IEnumerable<Student>> GetAllStudentsEnrolledInCourseAsync(Guid courseId, bool trackChanges)
            => await FindByCondition(c => c.Courses
            .FirstOrDefault(c => c.Id == courseId)!
            .Id == courseId, trackChanges)
            .OrderBy(e => e.UserName)
            .ToListAsync(); 

        public void EnrollForCourse(Guid courseId, Student student)
            => _context.Set<Course>().FirstOrDefault(c => c.Id == courseId)!.Students.Add(student);
        /*FindByCondition(c => c.Courses.FirstOrDefault(c => c.Id == courseId).Id == courseId)*/
        public async Task<bool> CheckToEnroll(Guid courseId, string studentId)
        {
            var enroll = await GetAllStudentsEnrolledInCourseAsync(courseId, false);
            var student = enroll.Where(e => e.Id == studentId).FirstOrDefault();
            if (student is not null)
            {
                return false;
            }
            return true;
        }
         

    }
}
