﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class VideoForCreateDto
    {
        public required string Title { get; set; }
        public IFormFile? File { get; set; }
    }
}
