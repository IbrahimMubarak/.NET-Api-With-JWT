using FirstJWTApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstJWTApp.Services
{
    public interface IAuthService
    {
        public Task<AuthModel> Register(RegisterModel model);
        public Task<AuthModel> Login(LoginModel model);
        public  Task<string> AddRole(AddRoleModel model);
    }
}
