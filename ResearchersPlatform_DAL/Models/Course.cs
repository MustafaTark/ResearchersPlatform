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
        public required string Name { get; set; }
        public required string Instructions { get; set; }
        public required string  Objectives { get; set; }
        public required double Price { get; set; }
        public ICollection<Section> Sections{ get; set; }
        public ICollection<Student> Students { get; set; }
        public required int Enrollments { get; set; } 
        public required string Hours { get; set; }
        public required string Brief { get; set; }
        public required string DriveLink { get; set; }
        [ForeignKey(nameof(Skill))]
        public int SkillId { get; set; }
        public Skill? SkillObj {  get; set; }

        public Course()
        {
            Sections=new HashSet<Section>();
            Students = new HashSet<Student>(); // NEW + MIGRATION
           
        }
    }
}
