using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Domain.Entites.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services.Token
{
    public class TokenHandler : ITokenHandler
    {
        readonly IConfiguration _configuration;
        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Application.DTOs.Token CreateAccessToken(int second,AppUser user)
        {
             Application.DTOs.Token token = new();
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            token.Expiration = DateTime.UtcNow.AddHours(3).AddSeconds(second);

            JwtSecurityToken jwtSecurityToken = new(
                   issuer: _configuration["Token:Issuer"],
                   audience: _configuration["Token:Audience"],
                   expires: token.Expiration,
                   notBefore: DateTime.UtcNow.AddHours(3),
                   signingCredentials: credentials,
                   claims: new List<Claim> { new(ClaimTypes.Name,user.UserName)}
               );
            JwtSecurityTokenHandler tokenHandler = new();
            token.AccessToken = tokenHandler.WriteToken(jwtSecurityToken);
         
            token.RefreshToken = CreateRefreshToken();
            return token;

        }

        public string CreateRefreshToken()
        {
            byte[] number = new byte[32];
            using RandomNumberGenerator random =  RandomNumberGenerator.Create();
            random.GetBytes(number);
            return Convert.ToBase64String(number);
        
        }
    }
}
