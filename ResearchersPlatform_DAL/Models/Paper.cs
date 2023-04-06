using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
    public class Paper
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Citation { get; set; }
        public required string Url { get; set; }

    }
}
