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
        public ICollection<Question> Questions { get; set; }
        public TimeSpan TimeLimit { get; set; }
        public bool IsSuccessed { get; set; } = false;
        public Quiz()
        {
            Questions= new HashSet<Question>();
        }
    }

}
