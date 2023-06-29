using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
    public class IdeaFile
    {
        public int ID { get; set; }
        public required string Path { get; set; }
        public required string Title { get; set; }
        [ForeignKey(nameof(Models.Researcher))]
        public Guid SenderId { get; set; }
        public Researcher? Researcher { get; set; }
        public DateTime Date { get; set; }
        [ForeignKey(nameof(Models.Idea))]
        public Guid IdeaId { get; set; }
        public Idea? Idea { get; set; }
        public bool IsSubmitedFile { get; set; }
    }
}
