using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class PrivateMessageDto
    {
        public required string Content { get; set; }
        public DateTime Date { get; set; }
        public string? SenderId { get; set; }
        public string? ReciverId { get; set; }
    }
}
