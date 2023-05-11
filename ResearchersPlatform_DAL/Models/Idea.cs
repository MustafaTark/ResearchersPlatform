using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
    public class Idea
    {
        public Idea()
        {
            Participants = new List<Researcher>();
            Tasks = new List<TaskIdea>();
            ExpertRequests = new List<ExpertRequest>();
        }
        public Guid Id { get; set; }
        public required string? Name { get; set; }
        public required int ParticipantsNumber { get; set; } 
        public required int MaxParticipantsNumber { get; set; } 

        [ForeignKey(nameof(Researcher))]
        public Guid? CreatorId { get; set; }
        public Researcher? ResearcherCreator { get; set; }
        public ICollection<Researcher> Participants { get; set; }
        public ICollection<TaskIdea> Tasks { get; set; }
        public ICollection<ExpertRequest> ExpertRequests { get; set; }
        
        [ForeignKey(nameof(Specality))]
        public int? SpecalityId {get; set; }
        public Specality? SpecalityObj { get; set; }
        
        [DataType(DataType.Time)]
        public DateTime Deadline { get; set; }
        public required bool IsCompleted { get; set; } = false;
       
        [ForeignKey(nameof(Topic))]
        public required int TopicId { get; set; }

        public Topic? TopicObject { get; set; }

    }
}
