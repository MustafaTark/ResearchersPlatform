using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
    public class TaskIdea
    {
        public TaskIdea()
        {
            Participants = new List<Researcher>();
            Files = new HashSet<TaskFile>();
        }
        public Guid Id { get; set; }
        public int ParticipantsNumber { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public DateTime Deadline { get; set; }
        public ICollection<TaskFile> Files { get; set; }
        public Progress Progress { get; set; }
        public ICollection<Researcher>? Participants { get; set; }
        [ForeignKey(nameof(Idea))]
        public Guid IdeaId { get; set; }
        public Idea? IdeaObject { get; set; }
        public bool IsCompleted = false;
    }
    public enum Progress
    {
        ASSIGNED,
        IN_PROGRESS,
        COMPLETED,
        CLOSED
        
    }
}
