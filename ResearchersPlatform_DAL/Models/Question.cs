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
        public required string Name { get; set; }
        public ICollection<Answer> Answers { get; set; }
        // public string CorrectAnswer = " ";
        [ForeignKey(nameof(Quiz))]
        public Guid QuizId { get; set; }
        public Quiz? QuizObject { get; set; }
        public int Score { get; set; }
        public Question()
        {
            Answers = new HashSet<Answer>();
        }
    }
}
