using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
    public class SectionQuiz : Quiz
    {
        [ForeignKey(nameof(Section))]
        public Guid SectionId { get; set; }
        public Section? SectionObj { get; set; }


    }
}
