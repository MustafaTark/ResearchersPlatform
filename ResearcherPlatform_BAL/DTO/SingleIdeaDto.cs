﻿using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class SingleIdeaDto
    {
        public Guid Id { get; set; }
        public required string? Name { get; set; }
        public required int ParticipantsNumber { get; set; }
        public required int MaxParticipantsNumber { get; set; }
        public Guid? CreatorId { get; set; }
        public ICollection<SingleResearcherDto>? Participants { get; set; }
        public int? SpecalityId { get; set; }
        public Specality? SpecalityObj { get; set; }
        public DateTime Deadline { get; set; }
        public required bool IsCompleted { get; set; } = false;
        public required int TopicId { get; set; }

        public Topic? TopicObject { get; set; }

    }
}
