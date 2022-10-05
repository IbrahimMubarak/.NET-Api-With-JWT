using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstJWTApp.Model
{
    public class AuthModel
    {
        public string Message { get; set; }
        public bool IsAuth { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Tok { get; set; }
        public List<string> Roles { get; set; }
        public DateTime ExpireOn { get; set; }
    }
}
