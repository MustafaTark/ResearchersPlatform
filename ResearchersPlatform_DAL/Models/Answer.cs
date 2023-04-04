using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
    public class Answer
    {
        public Guid Id { get; set; }
        public string? AnswerText { get; set; }
        [ForeignKey(nameof(Question))]
        public bool IsCorrectAnswer { get; set; }
        public Guid QuestionId { get; set; }
        
    }
}
