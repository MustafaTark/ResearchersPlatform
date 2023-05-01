using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class IdeaForCreateDTO
    {
        public string? Name { get; set; }
        //public int ParticipantsNumber { get; set; }
        public int MaxParticipantsNumber { get; set; }
        //public Guid CreatorId { get; set; }
        public int TopicId { get; set; }
        public int SpecalityId { get; set; }
        //public List<TopicsDto>? Topics { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Deadline { get; set; }

    }
}
