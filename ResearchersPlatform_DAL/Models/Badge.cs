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
        public string? ImageUrl { get; set; }
        public bool IsVisible { get; set; } = true;
        //public Guid ResearcherId {get;set;}
        //public Researcher Researcher{get;set;}


    }

}
