using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class ProblemFoCreateDto
    {
        public required string Description { get; set; }
        public required string StudentId { get; set; }
        public int ProblemCategoryId { get; set; }
    }
}
