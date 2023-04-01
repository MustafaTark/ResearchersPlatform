using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
    public class Course
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Instructions { get; set; }
        public string?  Objectives { get; set; }
        public double Price { get; set; }
        public List<Video> Videos { get; set; }
        public int Enroll { get; set; } 
        //Quiz Property
        public string? Hours { get; set; }
        public string? Brief { get; set; }
        [ForeignKey(nameof(Skill))]
        public int SkillId { get; set; }
        public Skill? SkillObj {  get; set; }
        public Course()
        {
            Videos=new List<Video>();
           
        }
    }
}
