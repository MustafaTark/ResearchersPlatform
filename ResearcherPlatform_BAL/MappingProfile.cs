using AutoMapper;
using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_BAL.ViewModels;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {

            CreateMap<StudentForRegisterDto, Student>();
            CreateMap<UserForLoginDto, User>();
            CreateMap<Student , StudentDto>();
            CreateMap<StudentForUpdateDto, Student>();

            CreateMap<Course,CourseDto>();
            CreateMap<CourseForCreationDto,Course>();
            CreateMap<CourseForUpdateDto,Course>();
            CreateMap<EnrollmentDto,Course>();

            CreateMap<EnrollmentDto,Student>();

            CreateMap<SectionQuiz, SectionQuizDto>().ForMember(c => c.TimeLimit,
                opt => opt.MapFrom(x => x.TimeLimit.ToString()));
            CreateMap<SectionQuizForCreateDto, SectionQuiz>().ForMember(a => a.TimeLimit,
                opt => opt.MapFrom(x => TimeSpan.Parse(x.TimeLimit!)));
            CreateMap<QuizResults, QuizResultsDto>();
            CreateMap<QuizResultsForCreateDto, QuizResults>();

            CreateMap<Question, QuestionDto>();
            CreateMap<QuestionForCreateDto, Question>();

            CreateMap<Answer, AnswerDto>();
            CreateMap<AnswerForCreateDto, Answer>();

            CreateMap<Section, SectionDto>();
            CreateMap<SectionForCreateDto, Section>();

            CreateMap<FinalQuiz, FinalQuizDto>();
            CreateMap<FinalQuizForCreateDto, FinalQuiz>();

            CreateMap<MessageDto, IdeaMessage>().ReverseMap();
            CreateMap<MessageDto, TaskMessage>().ReverseMap();
            CreateMap<PrivateMessageDto, PrivateMessage>().ReverseMap();
            CreateMap<Badge, BadgeDto>();
            CreateMap<Video, VideoDto>();
            CreateMap<IdeaForCreateDTO,Idea>();
            CreateMap<Idea, IdeaDto>();
            CreateMap<Idea, SingleIdeaDto>();
            CreateMap<IdeaForCreateDTO, Idea>();
            CreateMap<Topic, TopicsDto>();
            CreateMap<Specality, SpecialityDto>();
            CreateMap<SpecialityForCreationDto, Specality>();
            CreateMap<Paper, PaperDto>().ReverseMap();
            CreateMap<PaperForCreationDto, Paper>().ReverseMap();
            CreateMap<PaperForCreationDto, PaperDto>();
            CreateMap<Problem, ProblemDto>();
            CreateMap<ProblemFoCreateDto, Problem>();
            CreateMap<Researcher, ResearcherDto>();
            CreateMap<Researcher, SingleResearcherDto>();
            //CreateMap<VideoForCreateDto, Video>();
            CreateMap<RequestForCreationDto,RequestIdea>();
            CreateMap<RequestIdea,RequestDto>();
            CreateMap<InvitationForCreationDto,Invitation>();
            CreateMap<Invitation, InvitationDto>();
            CreateMap<TaskForCreateDto, TaskIdea>();
            CreateMap<TaskIdea, TaskDto>();
            CreateMap<TaskForParticipantsDto, TaskIdea>();
            CreateMap<AdminForRegisterDto, User>();
            CreateMap<SkillForCreationDto, Skill>();
            CreateMap<TopicForCreationDto, Topic>();
            CreateMap<ExpertRequestForCreateDto,ExpertRequest>();
            CreateMap<ExpertRequest, ExpertRequestDto>().ReverseMap();
            CreateMap<Nationality,NationalityDto>();
            CreateMap<ResponseForCreationDto, Response>();
            CreateMap<Response, ResponseDto>();
            CreateMap<StudentSpecialAccountsForCreationDto, Student>();
            CreateMap<ResearcherSpecialAccountsForCreationDto, Researcher>();

            CreateMap<IdeaFile, FileDto>();
            CreateMap<TaskFile, FileDto>();
        }
    }
}
