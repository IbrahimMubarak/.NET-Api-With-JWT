using FirstJWTApp.Helpers;
using FirstJWTApp.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FirstJWTApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _user;
        private readonly RoleManager<IdentityRole> _role;
        
        private readonly JWT _jwt;
        public AuthService(UserManager<ApplicationUser> user, IOptions<JWT> jwt, RoleManager<IdentityRole> role)
        {
            _user = user;
            _jwt = jwt.Value;
            _role = role;
        }

        
        public async Task<AuthModel> Register(RegisterModel model)
        {
            if (await _user.FindByEmailAsync(model.Email) is not null)
            {
                return new AuthModel()
                {
                    Message = "Email is already registered"
                };
            }
            if (await _user.FindByNameAsync(model.Username) is not null)
            {
                return new AuthModel()
                {
                    Message = "Username is already registered"
                };
            }

            ApplicationUser user = new ApplicationUser()
            {
                Firstname = model.FirstName,
                Lastname=model.LastName,
                Email=model.Email,
                UserName=model.Username
            };
            var result = await _user.CreateAsync(user,model.Password);
            if (!result.Succeeded)
            {
                return new AuthModel { Message = "Error" };
            }
            await _user.AddToRoleAsync(user, "User");

            var jwtSecurityToken = await CreateJwtToken(user);

            return new AuthModel
            {
                Email = user.Email,
                ExpireOn = jwtSecurityToken.ValidTo,
                IsAuth = true,
                Roles = new List<string> { "User" },
                Tok = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName
            };
        }

        public async Task<AuthModel> Login(LoginModel model)
        {
            var user = await _user.FindByEmailAsync(model.Email);
            if (user is null||!await _user.CheckPasswordAsync(user,model.Password) )
            {
                return new AuthModel()
                {
                    Message = "Wrong Email or Password"
                };
            }
            var jwtSecurityToken = await CreateJwtToken(user);
            var roles = await _user.GetRolesAsync(user);
            return new AuthModel
            {
                Email = user.Email,
                ExpireOn = jwtSecurityToken.ValidTo,
                IsAuth = true,
                Roles = roles.ToList(),
                Tok = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName
            };
        }

        public async Task<string> AddRole(AddRoleModel model)
        {
            var user = await _user.FindByIdAsync(model.Userid);

            if (user is null || !await _role.RoleExistsAsync(model.Role))
                return "Invalid user ID or Role";

            if (await _user.IsInRoleAsync(user, model.Role))
                return "User already assigned to this role";

            var result = await _user.AddToRoleAsync(user, model.Role);

            return result.Succeeded ? string.Empty : "Sonething went wrong";
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _user.GetClaimsAsync(user);
            var roles = await _user.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}

