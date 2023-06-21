using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class ResearcherSpecialAccountsForCreationDto
    {
        //[Range(0,3 , ErrorMessage ="Level Should be Between ( 0 - 3 )")]
        //public Level Level { get; set; }
        public int Points { get; set; }
        public int SpecalityId { get; set; }

    }
}
