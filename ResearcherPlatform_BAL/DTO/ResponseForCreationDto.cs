using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class ResponseForCreationDto
    {
        public required string Message { get; set; }
        public required string StudentId { get; set; }
        public required Guid ProblemId { get; set; }
    }
}
