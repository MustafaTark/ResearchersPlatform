using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
    public class Researcher : Student
    {
        public int Level { get; set; }
        public int Points { get; set; }
        public ICollection<Idea> Ideas { get; set; }
        public ICollection<Idea> IdeasLeader { get; set; }
        public ICollection<Task> Tasks { get; set; }    
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<Invitation> Invitations { get; set; }

        public Researcher() { 
            Ideas = new HashSet<Idea>();
            IdeasLeader = new HashSet<Idea>();
            Tasks = new HashSet<Task>();
            Notifications = new HashSet<Notification>();
            Invitations = new HashSet<Invitation>();
        }
    }
}
