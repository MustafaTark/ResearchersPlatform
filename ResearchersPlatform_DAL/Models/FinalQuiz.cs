using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
    public class FinalQuiz : Quiz
    {
        [ForeignKey(nameof(Skill))]
        public int SkillId { get; set; }
        public Skill? SkillObj { get; set; }
        public required double Price { get; set; }

    }
}
