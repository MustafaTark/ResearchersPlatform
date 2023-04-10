using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
    public class Section
    {
        public Guid Id { get; set; }

        [ForeignKey(nameof(Course))]
        public Guid? CourseId {get;set;}
        public Course? CourseObject { get; set; }
        [ForeignKey(nameof(Section))]
        public Guid? SectionId { get; set; }
        public ICollection<Video>? Videos { get; set; }
        public required string Name { get; set; }
    }
}
