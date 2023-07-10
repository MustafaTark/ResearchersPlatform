using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
   
    public class Student : User
    {
        public required string Firstname { get; set; }
        public required string Lastname { get; set; }
        public Gender Gender { get; set; }
        [Range(18, 70, ErrorMessage = "Age must be between (18 - 70)")]
        public int Age { get; set; }
        [ForeignKey(nameof(Models.Nationality))]
        public int NationalityId { get; set; }
        public Nationality? Nationality { get; set; }
        public Typee Type { get; set; }
        public string? GoogleSchoolerLink { get; set; }
        public string? OrcaId { get; set; }
        public bool IsMentor { get; set; }
        public  string? Bio { get; set; }
        public ICollection<Course> Courses { get; set; }
        
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
    public enum Typee
    {
        Student,
        Graduate,
        DoctorOrSpecialist,
        Other
    }
}
