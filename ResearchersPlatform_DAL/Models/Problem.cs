using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
    public class Problem
    {
        public Guid Id { get; set; }
        public required string Description { get; set; }
        [ForeignKey(nameof(Models.Student))]
        public required string StudentId { get; set; }
        public Student? Student { get; set; }
        [ForeignKey(nameof(Models.ProblemCategory))]
        public int ProblemCategoryId { get; set; }
        public ProblemCategory? ProblemCategory { get; set;}
    }
}
