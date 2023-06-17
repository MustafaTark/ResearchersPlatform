using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.DTO
{
    public class StudentForRegisterDto
    {
        [Required]
        public string? Firstname { get; set; }
        [Required]
        public string? Lastname { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
        [Required]
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int NationalityId { get; set; }
        public Gender Gender { get; set; }
        [Range(18, 70, ErrorMessage = "Age must be between (18 - 70)")]
        public int Age { get; set; }
    }
}
