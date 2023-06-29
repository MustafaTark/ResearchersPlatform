using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
    public class TaskFile
    {
        public int ID { get; set; }
        public required string Path { get; set; }
        public required string Title { get; set; }
        [ForeignKey(nameof(Models.TaskIdea))]
        public Guid TaskId { get; set; }
        public TaskIdea? Task { get; set; }
        [ForeignKey(nameof(Models.Researcher))]
        public Guid SenderId { get; set; }
        public Researcher? Researcher { get; set; }
        public DateTime Date { get; set; }
        public bool IsSubmitedFile { get; set; }
    }
}
