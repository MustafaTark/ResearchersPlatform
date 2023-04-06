using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class CourseForCreationDto
    {
        public required string Name { get; set; }
        public required string Instructions { get; set; }
        public required string Objectives { get; set; }
        public required double Price { get; set; }
        //public ICollection<Section> Sections { get; set; }
        public required string Hours { get; set; }
        public required string Brief { get; set; }
        public int SkillId { get; set; }


    }
}
