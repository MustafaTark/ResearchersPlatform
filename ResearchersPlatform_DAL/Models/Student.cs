using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
    public class Student : User
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public Gender Gender { get; set; }
        [Range(18, 70, ErrorMessage = "Age must be between (18 - 70)")]
        public int Age { get; set; }
       public ICollection<Course> Courses { get; set; }
        public int Trails { get; set; }
        public ICollection<Badge> Badges { get; set; }
        public Student() {
            Courses = new HashSet<Course>();
            Badges = new HashSet<Badge>();
        }
    }
    public enum Gender
    {
        MALE,
        FEMALE
    }
}
