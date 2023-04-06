using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
    public class Answer
    {
        public Guid Id { get; set; }
        public required string AnswerText { get; set; }
        [ForeignKey(nameof(Question))]
        public required bool IsCorrectAnswer { get; set; }
        public Guid QuestionId { get; set; }
        
    }
}
