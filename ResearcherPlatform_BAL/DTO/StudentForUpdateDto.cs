using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class StudentForUpdateDto
    {
        public required string Firstname { get; set; }
        public required string Lastname { get; set; }
        public Gender Gender { get; set; }
        [Range(18, 70, ErrorMessage = "Age must be between (18 - 70)")]
        public int Age { get; set; }
        public int NationalityId { get; set; }
        public Typee Type { get; set; }
        public string? GoogleSchoolerLink { get; set; }
    }
}
