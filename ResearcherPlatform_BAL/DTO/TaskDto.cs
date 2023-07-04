using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class TaskDto
    {
        public Guid Id { get; set; }
        public Guid IdeaId { get; set; }
        public int ParticipantsNumber { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public Progress Progress { get; set; }
        public DateTime Deadline { get; set; }
        public bool IsCompleted { get; set; }
    }
}
