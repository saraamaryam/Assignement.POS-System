
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using POS.API.Models.DTO;
using POS.API.Models.Entities;
using POS.API.Repositories.UserRepository;
namespace POS.API.Services
{
   
    public class TokenServices 
    {
        private const string SecretKey = "6B29FC40-CA47-1067-B31D-00DD010662DA"; 
        private readonly SymmetricSecurityKey _signingKey;
       
        public TokenServices(IConfiguration config)
        {
            _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
            if (string.IsNullOrEmpty(SecretKey) || SecretKey.Length < 32)
            {
                throw new InvalidOperationException("The secret key must be 32 characters long.");
            }
        }
        public string GenerateToken(LoginDTO user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
           
            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(new[]
                {
                        new Claim(ClaimTypes.Name, user.name),
                        new Claim(ClaimTypes.Role, user.role.ToString())
                    }),
                Expires = DateTime.UtcNow.AddDays(20),

                SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,
                ValidateIssuer = false,
                ValidateAudience = false
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);
            return principal;
        }
    }
}