using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
    public class IdeaMessage
    {
        public int Id { get; set; }
        public required string Content { get; set; }
        public DateTime Date { get; set; }
        [ForeignKey(nameof(Models.Researcher))]
        public Guid ResearcherId { get; set;}
        public Researcher? Researcher { get; set; }
        [ForeignKey(nameof(Models.Idea))]
        public Guid IdeaId { get; set;}
        public Idea? Idea { get; set; }
    }
}
