using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class TopicForCreationDto
    {
        public required string Name { get; set; }
        [Range(4, 5, ErrorMessage = "Can only be (4 .OR. 5)")]
        public int MinmumPoints { get; set; }
    }
}
