using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.ViewModels
{
    public class ResearcherRatePostViewModel
    {
        public Guid ResearcherId { get; set; }
        [Range(0, 10,ErrorMessage ="Rate Should be between 0 and 10")]
        public int Rate { get; set; }
    }
}
