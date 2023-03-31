using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
    public class Video
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        //public TimeOnly Duration { get; set; }
        public string? VideoUrl { get; set; }
        [ForeignKey(nameof(Course))]
        public Guid CourseId { get; set; }
    }
}
