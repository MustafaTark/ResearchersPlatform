using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
    public class Quiz
    {
        public Guid Id { get; set; }
        public int MaxScore { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }
        public ICollection<Question> Questions { get; set; }
        public TimeSpan TimeLimit { get; set; }
        [ForeignKey(nameof(Skill))]
        public int SkillId { get; set; }
        public double Price { get; set; }
        public bool IsSuccessed { get; set; } = false;
        public Quiz()
        {
            Questions= new List<Question>();
        }
    }

    public enum DifficultyLevel
    {
        Easy,
        Intermediate,
        Difficult
    }
}
