using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.ViewModels
{
    public class ResearcherViewModel
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public required string StudentId { get; set; }
        public Student? StudentObj { get; set; }
        public Level Level { get; set; }
        public int Points { get; set; }
        public Specality? Specality { get; set; }
        public ICollection<PaperDto>? Papers { get; set; }
        public ICollection<BadgeDto>? Badges { get; set; }
    }
}
