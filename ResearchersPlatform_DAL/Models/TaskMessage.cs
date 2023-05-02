using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
    public class TaskMessage
    {
        public int Id { get; set; }
        public required string Content { get; set; }
        public DateTime Date { get; set; }
        [ForeignKey(nameof(Models.Researcher))]
        public Guid ResearcherId { get; set; }
        public Researcher? Researcher { get; set; }
        [ForeignKey(nameof(Models.TaskIdea))]
        public Guid TaskIdeaId { get; set; }
        public TaskIdea? TaskIdea { get; set; }
    }
}
