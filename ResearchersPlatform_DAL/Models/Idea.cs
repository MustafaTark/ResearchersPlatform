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
            Participants = new List<User>();
            Tasks = new List<Task>();
        }
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int ParticipantsNumber { get; set; } 
        public int MaxParticipantsNumber { get; set; } 

        [ForeignKey(nameof(User))]
        public Guid? CreatorId { get; set; }
        public User? UserCreator { get; set; }
        public ICollection<User>? Participants { get; set; }
        public ICollection<Task> Tasks { get; set; }
        
        [ForeignKey(nameof(Specality))]
        public Guid? SpecalityId {get; set; }
        public Specality? Specality { get; set; }
        
        [DataType(DataType.Time)]
        public DateTime Deadline { get; set; }
        public bool IsCompleted { get; set; } = false;
    }
}
