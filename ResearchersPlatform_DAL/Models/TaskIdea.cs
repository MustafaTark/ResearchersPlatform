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
        }
        public Guid Id { get; set; }
        public int ParticipantsNumber { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime Deadline { get; set; }
        public Progress Progress { get; set; }
        public ICollection<Researcher>? Participants { get; set; }
        [ForeignKey(nameof(Idea))]
        public Guid IdeaId { get; set; }
        public Idea? IdeaObject { get; set; }
    }
    public enum Progress
    {
        NOT_STARTED,
        IN_PROGRESS,
        COMPLETED
        
    }
}
