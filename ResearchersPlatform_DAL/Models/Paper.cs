using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public Researcher? ResearcherObject { get; set; }
        
        [ForeignKey(nameof(ResearcherId))]
        public Guid ResearcherId { get;set; }

    }
}
