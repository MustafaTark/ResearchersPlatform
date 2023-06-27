using Microsoft.AspNetCore.Http;
using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_BAL.RequestFeatures;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Contracts
{
    public interface IStudentRepository
    {
        void UpdateStudent(Student student);
        void DeleteStudent(Student student);
        Task<Student?> GetStudentByIdAsync(string studentId, bool trackChanges);
        Task<IEnumerable<Student?>> GetAllStudentsAsync(StudentParamters paramters);
        void CreateTrails(string studentId);
        Task<IEnumerable<Student>> GetAllStudentsEnrolledInCourseAsync(Guid courseId , bool trackChanges);
        void EnrollForCourse(Guid courseId, Student student);
        Task<bool> CheckToEnroll(Guid courseId, string studentId);
        Task<IEnumerable<NationalityDto>> GetAllNationalitiesAsync(bool trackChanges);

    }
}
