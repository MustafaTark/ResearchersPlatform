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
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public Nationality? Nationality { get; set; }
        public Typee Type { get; set; }
        public string? GoogleSchoolerLink { get; set; }
        public string? OrcaId { get; set; }
        //public List<Course>? Courses { get; set; }
        //public int Trails { get; set; }
        public List<BadgeDto>? Badges { get; set; }
    }
}
