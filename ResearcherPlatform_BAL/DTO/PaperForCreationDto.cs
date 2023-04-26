using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class PaperForCreationDto
    {
        public required string Name { get; set; }
        public required string Citation { get; set; }
        public required string Url { get; set; }
    }
}
