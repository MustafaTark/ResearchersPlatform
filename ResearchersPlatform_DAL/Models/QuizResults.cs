using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
    public class QuizResults
    {
        public Guid Id { get; set; }
        [ForeignKey(nameof(Quiz))]
        public Guid QuizId { get; set; }
        public Quiz? QuizObj { get; set; }
        [ForeignKey(nameof(Student))]
        public required string StudentId { get; set; }
        public Student? StudentObj { get; set; }
        public int Score { get; set; }
        public bool IsSuccessed { get; set; } = false;
    }
}
