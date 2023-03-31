using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Models
{
    public class User : IdentityUser
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public Gender Gender { get; set; }
        public string? City { get; set; }
        public string? Country{ get; set; }
        //public string? Speciality { get; set; }  →

        [Range(20,70, ErrorMessage ="Age must be between (20 - 70)")]
        public int Age { get; set; }
    }
    public enum Gender
    {
        MALE,
        FEMALE
    }
}
