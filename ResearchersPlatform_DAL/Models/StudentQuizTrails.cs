using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
    public class StudentQuizTrails
    {
        public Guid Id { get; set; }
        [ForeignKey(nameof(Skill))]
        public int SkillId { get; set; }
        public Skill? Skill { get; set; }
        [ForeignKey(nameof(Student))]
        public required string StudentId { get; set; }
        public Student? StudentObj { get; set; }
        public byte Trails { get; set; } = 2;
    }
}
