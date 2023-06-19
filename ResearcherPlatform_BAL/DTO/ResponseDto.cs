using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class ResponseDto
    {
        public Guid ID { get; set; }
        public required string Message { get; set; }
        public required string StudentId { get; set; }
        public ProblemDto? Problem { get; set; }

    }
}
