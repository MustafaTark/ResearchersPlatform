using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.RequestFeatures
{
    public class ResearcherParamters : RequestParamters
    {
        public string? SearchTerm { get; set; }
        public Level? Level{ get; set; }
        public int Specality { get; set; }
    }
}
