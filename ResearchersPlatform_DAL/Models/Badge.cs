using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
    public class Badge
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public bool IsVisible { get; set; } = true;
        [ForeignKey(nameof(Student))]
        public Guid StudentId { get;set;}
        public Student? StudentObj{get;set;}


    }

}
