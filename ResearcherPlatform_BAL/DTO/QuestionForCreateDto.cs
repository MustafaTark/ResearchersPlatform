using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class QuestionForCreateDto
    {
        public required string Name { get; set; }
        public ICollection<AnswerForCreateDto>? Answers { get; set; }
        public int Score { get; set; }
    }
}
