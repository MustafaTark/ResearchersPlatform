using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.RequestFeatures
{
    public class IdeasParamters : RequestParamters
    {
        public string? SearchTerm { get; set; }
        public int Topic { get; set; }
        public int Specality { get; set; }
        public int Month { get; set; }
    }
}
