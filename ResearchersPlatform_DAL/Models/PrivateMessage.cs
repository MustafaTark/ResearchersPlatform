using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
    public class PrivateMessage
    {
        public Guid Id { get; set; }
        public required string Content { get; set; }
        public DateTime Date { get; set; }
        [ForeignKey(nameof(User))]
        public string? SenderId { get; set; }
        public User? Sender { get; set; }
        [ForeignKey(nameof(User))]
        public string? ReciverId { get; set; }
        public User? Reciver { get; set; }
    }
}
