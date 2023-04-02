using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
    public class Request
    {
        public Guid Id { get; set; }

        [ForeignKey(nameof(Idea))]
        public Guid IdeaId { get; set; }
        public Idea? IdeaObject { get; set; }
        
        [ForeignKey(nameof(Researcher))]
        public Guid ResearcherId { get; set; }
        public bool IsAccepted { get; set; } = false;
        //M-M
    }
}
