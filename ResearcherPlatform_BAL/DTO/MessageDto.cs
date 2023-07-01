using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class MessageDto
    {
        public required string Content { get; set; }
        public DateTime Date { get; set; }
        public Guid ResearcherId { get; set; }
    }
}
