using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
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
        private ISectionQuizRepository _sectionQuiz;
        private ISectionRepository _section;
        private IFinalQuizRepository _finalQuiz;
        private ISpecialityRepository _speciality;
        private IPaperRepository _paper;
        private IRequestRepository _request;
        private IInvitationRepository _invitation;
        private IChatRepository _chat;
        private IAdminRepository _admin;
        private IProblemRepository _problem;
        private IResponseRepository _response;
        private IExpertRequestRepository _expertRequest;
        private readonly IMemoryCache _memoryCache;
        private readonly IMapper _mapper;
        
        public RepositoryManager(AppDbContext context 
            , IStudentRepository student
            , ICourseRepository course
            , IIdeaRepository idea
            , ITaskRepository task
            , IResearcherRepository researcher
            , ISectionQuizRepository sectionQuiz
            , IMapper mapper
            , ISectionRepository section
            , IFinalQuizRepository finalQuiz
            , ISpecialityRepository speciality
            , IPaperRepository paper
            , IRequestRepository request
            , IInvitationRepository invitation
            , IChatRepository chat, IMemoryCache memoryCache
            , IAdminRepository admin
            ,IProblemRepository problem
            ,IResponseRepository response
            ,IExpertRequestRepository expertRequest)
        {
            _context = context;
            _student = student;
            _course = course;
            _idea = idea;
            _task = task;
            _researcher = researcher;
            _sectionQuiz = sectionQuiz;
            _mapper = mapper;
            _section = section;
            _finalQuiz = finalQuiz;
            _speciality = speciality;
            _paper = paper;
            _request = request;
            _invitation = invitation;
            _chat = chat;
            _memoryCache = memoryCache;
            _admin = admin;
            _problem = problem;
            _response = response;
            _expertRequest = expertRequest;
        }
        public IStudentRepository Student
        {
            get
            {
                //null-coalescing operator
                _student ??= new StudentRepository(_context,_mapper);
                
                return _student;
            }
        }
        public ICourseRepository Course
        {
            get
            {
                _course ??= new CourseRepository(_context,_memoryCache);
                return _course;
            }
        }
        public IIdeaRepository Idea
        {
            get
            {
                _idea ??= new IdeaRepository(_context,_mapper);
                return _idea;
            }
        }
        public ITaskRepository Task
        {
            get
            {
                _task ??= new TaskRepository(_context,_mapper);
                return _task;
            }
        }
        public IResearcherRepository Researcher
        {
            get
            {
                _researcher ??= new ResearcherRepository(_context,_mapper);
                return _researcher;
            }
        }

        public ISectionQuizRepository SectionQuiz
        {
            get
            {
                _sectionQuiz ??= new SectionQuizRepository(_context,_mapper);
                return _sectionQuiz;
            }
        }
        public IFinalQuizRepository FinalQuiz
        {
            get
            {
                _finalQuiz ??= new FinalQuizRepository(_context, _mapper);
                return _finalQuiz;
            }
        }

        public ISectionRepository Section
        {
            get
            {
                _section ??= new SectionRepository(_context, _mapper,_memoryCache);
                return _section;
            }
        }
        public ISpecialityRepository Speciality
        {
            get
            {
                _speciality ??= new SpecialityRepository(_context, _mapper);
                return _speciality;
            }
        }
        public IPaperRepository Paper
        {
            get
            {
                _paper ??= new PaperRepository(_context, _mapper);
                return _paper;
            }
        }
        public IRequestRepository Request
        {
            get
            {
                _request ??= new RequestRepository(_context, _mapper);
                return _request;
            }
        }
        public IInvitationRepository Invitation
        {
            get
            {
                _invitation ??= new InvitationRepository(_context, _mapper);
                return _invitation;
            }
        } 
        public IChatRepository Chat
        {
            get
            {
                _chat ??= new ChatRepository(_context, _mapper);
                return _chat;
            }
        }
        public IAdminRepository Admin
        {
            get
            {
                _admin ??= new AdminRepository(_context, _mapper);
                return _admin;
            }
        }

        public IProblemRepository Problem
        {
            get
            {
                _problem ??= new ProblemRepository(_context, _mapper);
                return _problem;
            }
        }
        public IResponseRepository Response
        {
            get
            {
                _response ??= new ResponseRepository(_context, _mapper);
                return _response;
            }
        }
        public IExpertRequestRepository ExpertRequest
        {
            get
            {
                _expertRequest ??= new ExpertRequestRepository(_context, _mapper);
                return _expertRequest;
            }
        }

        public Task SaveChangesAsync() => _context.SaveChangesAsync();


    }
}
