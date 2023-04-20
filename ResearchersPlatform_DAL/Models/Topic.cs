﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int MinmumPoints { get; set; }
    }
}
