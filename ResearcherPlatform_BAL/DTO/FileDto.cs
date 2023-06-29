using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class FileDto
    {
        public int ID { get; set; }
        public required string Path { get; set; }
        public required string Title { get; set; }
        public Guid SenderId { get; set; }
    }
}
