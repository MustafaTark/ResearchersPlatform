using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
    public class Researcher : Student
    {
        public Level Level { get; set; }
        public int Points { get; set; }
        public ICollection<Idea> Ideas { get; set; }
        public ICollection<Idea> IdeasLeader { get; set; }
        public ICollection<TaskIdea> Tasks { get; set; }    
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<Invitation> Invitations { get; set; }
        
        [ForeignKey(nameof(Specality))]
        public int SpecalityId { get; set; }
        public Specality? SpecalityObject { get; set; }
        public ICollection<Paper> Papers { get; set; }

        public Researcher() { 
            Ideas = new HashSet<Idea>();
            IdeasLeader = new HashSet<Idea>();
            Tasks = new HashSet<TaskIdea>();
            Notifications = new HashSet<Notification>();
            Invitations = new HashSet<Invitation>();
            Papers = new HashSet<Paper>();
        }

    }
    public enum Level
    {
        Beginner,
        Intermediate,
        Professional,
        Expert
        }
}
