using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
    
    public class Question
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public List<Answer> Answers { get; set; }
        [ForeignKey(nameof(Answer))]
        public Guid CorrectAnswerId { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }
        public int Score { get; set; }
        public Question()
        {
            Answers = new List<Answer>();
        }
    }
}
