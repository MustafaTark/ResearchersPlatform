using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class QuestionDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public ICollection<AnswerDto>? Answers { get; set; }
        public int Score { get; set; }
    }
}
