using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class SectionForCreateDto
    {
        public string? Name { get; set; }
        public Guid? CourseId { get; set; }
        public ICollection<VideoForCreateDto>? Videos { get; set; }
    }
}
