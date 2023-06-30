﻿using Microsoft.AspNetCore.Identity;
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
        public string ImageUrl { get; set; } = "NotFound";
    }
   
}
