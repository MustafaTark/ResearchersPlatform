using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class InvitationForCreationDto
    {
        public List<Guid>? ResearchersIds { get; set; }
    }
}
