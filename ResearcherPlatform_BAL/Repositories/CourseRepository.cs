﻿using Microsoft.EntityFrameworkCore;
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
        public CourseRepository(AppDbContext context):base(context) 
        { 
        }

        public void CreateCourse(Course course) => Create(course);
        public void UpdateCourse(Course course) => Update(course);
        public void DeleteCourse(Course course) => Delete(course);
        public async Task<Course?> GetCourseByIdAsync(Guid courseId, bool trackChanges)
        {
            
                    return await FindByCondition(c => c.Id == courseId, trackChanges)
                                .Include(v => v.Sections)
                                .Include(c=>c.SkillObj)
                                .FirstOrDefaultAsync(); 
        }
        public async Task<IEnumerable<Course?>> GetAllCoursesAsync()
         =>await FindAll(trackChanges: false).Include(c=>c.SkillObj).ToListAsync();
               
        
        public async Task<IEnumerable<Course?>> GetAllCoursesForStudentAsync(string studentId, bool trackChanges)
           =>await FindByCondition(s => s.Students
                                    .FirstOrDefault(e => e.Id == studentId)!
                                    .Id == studentId, trackChanges)
                                    .Include(c => c.SkillObj)
                                    .ToListAsync();
    }
}
