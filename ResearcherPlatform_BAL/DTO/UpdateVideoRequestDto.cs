using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class UpdateVideoRequestDto
    {
        public IFormFile? NewVideoFile { get; set; }
        public string? NewTitle { get; set; }
    }
}
