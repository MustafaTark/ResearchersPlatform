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
        public Level Level { get; set; }
        public int Points { get; set; }
        public ICollection<Idea>? Ideas { get; set; }
        public ICollection<Idea>? IdeasLeader { get; set; }
        public Specality? Specality { get; set; }
        public ICollection<Paper>? Papers { get; set; }
        public ICollection<Badge>? Badges { get; set; }
    }
}
