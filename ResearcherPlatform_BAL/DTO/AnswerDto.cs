using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class AnswerDto
    {
        public Guid Id { get; set; }
        public required string AnswerText { get; set; }
     
    }

}
