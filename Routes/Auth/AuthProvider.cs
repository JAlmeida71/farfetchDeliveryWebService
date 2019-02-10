using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models.BussinessModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API
{
    public class AuthProvider
    {
        private IConfiguration config;

        public AuthProvider(IConfiguration iConfig) => config = iConfig;

        public JwtSecurityToken CreateToken(User model)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub , model.Username),
                new Claim(ClaimTypes.PrimarySid, model.ID.ToString()),
                new Claim(ClaimTypes.Role, model.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Tokens:Key"])); 
            var sigCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha384);

            var token = new JwtSecurityToken(
                issuer: config["Tokens:Issuer"],
                audience: config["Tokens:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), 
                signingCredentials: sigCredentials
                );

            return token;
        }
    }
}
