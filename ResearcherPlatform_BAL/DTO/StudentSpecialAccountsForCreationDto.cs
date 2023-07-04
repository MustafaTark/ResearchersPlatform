using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class StudentSpecialAccountsForCreationDto
    {
        public  required string Firstname { get; set; }
        public required string Lastname { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public required string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public int NationalityId { get; set; }
        public Gender Gender { get; set; }
        [Range(18, 70, ErrorMessage = "Age must be between (18 - 70)")]
        public int Age { get; set; }
        public Typee Type { get; set; }
        public bool IsMentor { get; set; }
        public string? Bio { get; set; }

    }
}
