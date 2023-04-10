using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class SectionQuizDto
    {
        public Guid Id { get; set; }
        public ICollection<QuestionDto>? Questions { get; set; }
        public string? TimeLimit { get; set; }
        public Guid SectionId { get; set; }

    }
}
