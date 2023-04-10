using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class AnswerForCreateDto
    {
        public  string? AnswerText { get; set; }
        public  bool IsCorrectAnswer { get; set; }
    }
}
