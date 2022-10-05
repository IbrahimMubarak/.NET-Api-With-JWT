using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FirstJWTApp.Model
{
    public class RegisterModel
    {
        [Required,MaxLength(50),MinLength(3)]
        public string FirstName { get; set; }
        [Required, MaxLength(50), MinLength(3)]
        public string LastName { get; set; }
        [Required, MaxLength(50), MinLength(3)]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
