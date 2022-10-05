﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FirstJWTApp.Model
{
    public class AddRoleModel
    {
        [Required]
        public string Userid { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
