using ResearchersPlatform_BAL.Contracts;
using ResearchersPlatform_DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly AppDbContext _context;
        private IStudentRepository _student;
        private ICourseRepository _course;
        private IIdeaRepository _idea;
        private ITaskRepository _task;
        private IResearcherRepository _researcher;

        public RepositoryManager(AppDbContext context 
            , IStudentRepository student
            , ICourseRepository course
            , IIdeaRepository idea
            , ITaskRepository task
            , IResearcherRepository researcher)
        {
            _context = context;
            _student = student;
            _course = course;
            _idea = idea;
            _task = task;
            _researcher = researcher;
        }
        public IStudentRepository Student
        {
            get
            {
                //null-coalescing operator
                _student ??= new StudentRepository(_context);
                
                return _student;
            }
        }
        public ICourseRepository Course
        {
            get
            {
                _course ??= new CourseRepository(_context);
                return _course;
            }
        }
        public IIdeaRepository Idea
        {
            get
            {
                _idea ??= new IdeaRepository(_context);
                return _idea;
            }
        }
        public ITaskRepository Task
        {
            get
            {
                _task ??= new TaskRepository(_context);
                return _task;
            }
        }
        public IResearcherRepository Researcher
        {
            get
            {
                _researcher ??= new ResearcherRepository(_context);
                return _researcher;
            }
        }
        public Task SaveChangesAsync() => _context.SaveChangesAsync();


    }
}
