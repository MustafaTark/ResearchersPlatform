﻿using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class InvitationDto
    {
        public Guid Id { get; set; }
        public Guid IdeaId { get; set; }
        public Guid ResearcherId { get; set; }
        public bool IsAccepted { get; set; }
    }
}
