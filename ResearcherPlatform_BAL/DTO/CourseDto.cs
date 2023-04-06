using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class CourseDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Instructions { get; set; }
        public string? Objectives { get; set; }
        public string? Hours { get; set; }
        public string? Brief { get; set; }

    }
}
