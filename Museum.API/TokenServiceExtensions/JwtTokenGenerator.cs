using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Museum.API.TokenService
{
    public static class JwtTokenGenerator
    {
        // WARNING: This is just for demo purpose
        public static string Generate(string userName, string name, string role, string issuer, string key)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserName", userName)
            };

            if (role.Equals("admin"))
            {
                claims.Add(new Claim(ClaimTypes.Role, "admin"));
            }
            if (role.Equals("super-user"))
            {
                claims.Add(new Claim(ClaimTypes.Role, "super-user"));
            }
            if (role.Equals("user"))
            {
                claims.Add(new Claim(ClaimTypes.Role, "user"));
            }
            if (role.Equals("guest"))
            {
                claims.Add(new Claim(ClaimTypes.Role, "guest"));
            }


            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer,
                issuer,
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);
            
            Console.WriteLine("+++++++++++++++ " + token);

            return new JwtSecurityTokenHandler().WriteToken(token);
            Console.WriteLine(token);
        }
    }
}
