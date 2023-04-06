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
        public Badge()
        {
            Students= new HashSet<Student>();
        }
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required bool IsVisible { get; set; } = true;

        public ICollection<Student> Students{get;set;}



    }

}
