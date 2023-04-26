using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class IdeaDto
    {
        public Guid Id { get; set; }
        public required string? Name { get; set; }
        public required int ParticipantsNumber { get; set; }
        public required int MaxParticipantsNumber { get; set; }
        public Guid? CreatorId { get; set; }
        public Researcher? ResearcherCreator { get; set; }
        public ICollection<Researcher>? Participants { get; set; }
        public ICollection<TaskIdea>? Tasks { get; set; }
        public Guid? SpecalityId { get; set; }
        public Specality? SpecalityObj { get; set; }
        public DateTime Deadline { get; set; }
        public required bool IsCompleted { get; set; } = false;
        public required int TopicId { get; set; }

        public TopicsDto? TopicObject { get; set; }

    }
}
