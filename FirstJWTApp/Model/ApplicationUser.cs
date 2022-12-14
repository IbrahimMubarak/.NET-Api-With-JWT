using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FirstJWTApp.Model
{
    public class ApplicationUser:IdentityUser
    {
        [Required,MaxLength(50),MinLength(3)]
        public string Firstname { get; set; }
        [Required, MaxLength(50), MinLength(3)]
        public string Lastname { get; set; }
    }
}
