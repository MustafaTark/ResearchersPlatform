using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
    public class CourseRepository : GenericRepository<Course> , ICourseRepository
    {
        private readonly IMemoryCache _memoryCache;
        public CourseRepository(AppDbContext context, IMemoryCache memoryCache):base(context) 
        { 
            _memoryCache = memoryCache;
        }

        public void CreateCourse(Course course) => Create(course);
        public void UpdateCourse(Course course) => Update(course);
        public void DeleteCourse(Course course) => Delete(course);
        public async Task<Course?> GetCourseByIdAsync(Guid courseId, bool trackChanges)
        {
            string key = $"course:{courseId}";
            var course = await _memoryCache.GetOrCreateAsync(
                key,
                async entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
                    return await FindByCondition(c => c.Id == courseId, trackChanges)
                                .Include(v => v.Sections)
                                .FirstOrDefaultAsync();
                }
              );
            return course!;
            
        }
        public async Task<IEnumerable<Course?>> GetAllCoursesAsync()
        {
            string key = "courses";
            var courses = await _memoryCache.GetOrCreateAsync(
                key,
                async entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
                   return await FindAll(trackChanges: false).ToListAsync();
                }
              );
            return courses!;
        }
        public async Task<IEnumerable<Course?>> GetAllCoursesForStudentAsync(string studentId, bool trackChanges)
        {
            {
                string key = $"coursesTo:{studentId}";
                var courses = await _memoryCache.GetOrCreateAsync(
                    key,
                    async entry =>
                    {
                        entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
                        return await FindByCondition(s => s.Students
                                    .FirstOrDefault(e => e.Id == studentId)!
                                    .Id == studentId, trackChanges)
                                    .ToListAsync();
                    }
                  );
                return courses!;
            }
        }
     
    }
}
