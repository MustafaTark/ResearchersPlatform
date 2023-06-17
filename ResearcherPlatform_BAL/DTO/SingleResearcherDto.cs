using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class SingleResearcherDto
    {
        public Guid Id { get; set; }
        public StudentDto? StudentObj { get; set; }
        public int Points { get; set; }
        public string Level { get; set; }
        public Specality? SpecalityObject { get; set; }

    }
}
