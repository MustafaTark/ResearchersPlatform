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
        }
    }
}
