using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class StudentDto
    {
        public string? Id { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }
        //public List<Course>? Courses { get; set; }
        //public int Trails { get; set; }
        //public List<Badge>? Badges { get; set; }
    }
}
