using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
    public class Response
    {
        public Guid Id { get; set; }
        public required string Message { get; set; }

        [ForeignKey(nameof(Models.Student))]
        public required string StudentId { get; set; }
        public Student? Student { get; set; }
        
        [ForeignKey(nameof(Models.Problem))]
        public required Guid ProblemId { get; set; }
        public Problem? Problem { get; set; }
    }
}
