using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class TaskForCreateDto
    {
        public int ParticipantsNumber { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public DateTime Deadline { get; set; }
        //public List<Guid>? ParticipantsIds { get; set; }

    }
}
