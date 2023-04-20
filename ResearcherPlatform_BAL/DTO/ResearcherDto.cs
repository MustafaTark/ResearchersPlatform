using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class ResearcherDto
    {
        public Guid Id { get; set; }
        public required string StudentId { get; set; }
        public StudentDto? Student { get; set; }
        public Level Level { get; set; }
        public int Points { get; set; }
        public ICollection<Idea>? Ideas { get; set; }
        public ICollection<Idea>? IdeasLeader { get; set; }
        public Specality? SpecalityObject { get; set; }
        public ICollection<Paper>? Papers { get; set; }
    }
}
