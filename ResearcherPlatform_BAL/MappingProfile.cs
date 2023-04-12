using AutoMapper;
using ResearchersPlatform_BAL.DTO;
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

            CreateMap<Video, VideoDto>();
            //CreateMap<VideoForCreateDto, Video>();
        }
    }
}
