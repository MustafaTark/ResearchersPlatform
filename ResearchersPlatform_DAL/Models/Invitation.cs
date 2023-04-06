using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
    public class Invitation
    {
        public Invitation()
        {
            Researchers = new HashSet<Researcher>();
        }
        public Guid Id { get; set; }
        
        [ForeignKey(nameof(Idea))]
        public Guid IdeaId { get; set; }
        public Idea? IdeaObject { get; set; }
        public bool IsAccepted { get; set; } = false;
        public ICollection<Researcher> Researchers { get; set; }


    }
}
