using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class QuizResultsForCreateDto
    {
        public Guid QuizId { get; set; }
        public string? StudentId { get; set; }
        public int Score { get; set; }
    }
}
