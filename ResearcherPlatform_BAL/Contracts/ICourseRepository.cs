using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Contracts
{
    public interface ICourseRepository
    {
        void CreateCourse(Course course);
        void UpdateCourse(Course course);
        void DeleteCourse(Course course);
        Task<Course?> GetCourseByIdAsync(Guid courseId , bool trackChanges);
        Task<IEnumerable<Course?>> GetAllCoursesAsync();
        Task<IEnumerable<Course?>> GetAllCoursesForStudentAsync(string studentId , bool trackChanges);
    }
}
