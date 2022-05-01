using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using SysOT.Models;
using System.Linq;

namespace SysOT.Services
{
    public interface IEncService
    {
        string EncryptPassword(string password);
        string GenerateToken(User user);
    }
    public class EncService : IEncService
    {
        private byte[] salt;
        private string key;
        private IConfiguration configuration;
        public EncService(IConfiguration _configuration)
        {
            configuration = _configuration;
            salt = Encoding.UTF8.GetBytes(_configuration["Database:Salt"].ToString());
            key = _configuration["JWT:Key"];
        }

        public string EncryptPassword(string password)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password,
                salt,
                KeyDerivationPrf.HMACSHA256,
                256,
                16
            ));
        }
        public string GenerateToken(User user){
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var tokenDescriptor = new SecurityTokenDescriptor(){
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.NameIdentifier,user.Id),
                    new Claim("Roles", string.Join(",",user.Roles))
                }),
                Expires = DateTime.UtcNow.AddHours(int.Parse(configuration["JWT:TokenLifeExpectancyInHours"].ToString())),
                SigningCredentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256),
                Issuer = configuration["JWT:Issuer"],
                Audience = configuration["JWT:Audience"]
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}