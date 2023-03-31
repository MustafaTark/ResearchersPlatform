using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
    public class Task
    {
        public Task()
        {
            Participants = new List<User>();
        }
        public Guid Id { get; set; }
        public int ParticipantsNumber { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        [DataType(DataType.Time)]
        public DateTime Deadline { get; set; }
        public Progress Progress { get; set; }

        public ICollection<User>? Participants { get; set; }
        [ForeignKey(nameof(Idea))]
        public Guid IdeaId { get; set; }
        public Idea? IdeaObject { get; set; }

        //TODO..
        //Task Creator 



    }
    public enum Progress
    {
        NOT_STARTED,
        IN_PROGRESS,
        COMPLETED
        
    }
}
