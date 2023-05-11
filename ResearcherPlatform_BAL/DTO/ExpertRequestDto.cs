using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class ExpertRequestDto
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public Guid IdeaId { get; set; }
        public Guid ParticipantId { get; set; }

    }
}
