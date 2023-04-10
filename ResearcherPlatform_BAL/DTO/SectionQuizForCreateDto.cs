using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class SectionQuizForCreateDto
    {
        public Guid SectionId { get; set; }
        public string? TimeLimit { get; set; }
        public int MaxScore { get; set; }
        public ICollection<QuestionForCreateDto>? Questions { get; set; }
       
    }
}
