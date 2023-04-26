using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class IdeaForCreateDTO
    {
        public string? Name { get; set; }
        public int ParticipantsNumber { get; set; }
        public int MaxParticipantsNumber { get; set; }
        //public Guid CreatorId { get; set; }
        public int TopicId { get; set; }
       // public int? SpecalityId { get; set; }
        //public List<TopicsDto>? Topics { get; set; }

    }
}
