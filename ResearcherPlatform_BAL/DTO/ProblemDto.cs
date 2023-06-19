using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class ProblemDto
    {
        public Guid Id { get; set; }
        public required string Description { get; set; }
        public required string StudentId { get; set; }
        //public int ProblemCategoryId { get; set; }
        public ProblemCategory? ProblemCategory { get; set; }
    }
}
