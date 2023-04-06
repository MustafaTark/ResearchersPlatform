using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
    public class Notification
    {
        public Notification()
        {
            Researchers = new HashSet<Researcher>();
        }
        public Guid Id { get; set; }

        public bool IsRead { get; set; } = false;

        [Required(ErrorMessage = "Notification Content is a Required Field!")]
        public required string Content { get; set; }
        public ICollection<Researcher>? Researchers { get; set; }
    }
}
