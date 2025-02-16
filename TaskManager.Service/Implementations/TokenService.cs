using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Entities;
using TaskManager.Data.Entities.Abstracts;
using TaskManager.Service.Helpers;
using TaskManager.Service.Interfaces;

namespace TaskManager.Service.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly JwtOptions jwtOptions;

        public TokenService(JwtOptions jwtOptions)
        {
            this.jwtOptions = jwtOptions;
        }
        public string GenerateAccessToken(UserView person)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtOptions.Issuer,
                Audience = jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
                                    SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.NameIdentifier, person.ID.ToString()),
                    new(ClaimTypes.Name, person.Username),
                    new(ClaimTypes.Role, person.Role),
                }),
                Expires = DateTime.UtcNow.AddMinutes(jwtOptions.LifeTime),
            };

            // Generate Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);
            return accessToken;
        }
    }
}
