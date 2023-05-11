using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
    public class ExpertRequest
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        [ForeignKey(nameof(Idea))]
        public Guid IdeaId { get; set; }
        public Idea? IdeaObject { get; set; }
        [ForeignKey(nameof(Researcher))]
        public Guid ParticipantId { get; set; }
        public Researcher? ResearcherObject { get; set; }
    }
}
