using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ModelsLibrary.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CLabManager_API.Services
{
    public class JwtService
    {
        private const int EXPIRATION_MINUTES = 10;
        
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AuthenticationResponse CreateToken(IdentityUser user)
        {
            var expiration = DateTime.UtcNow.AddMinutes(EXPIRATION_MINUTES);
            var token = CreateJwtToken(CreateClaims(user),expiration,CreateSigningCredentials());

            var tokenHandler = new JwtSecurityTokenHandler();
            return new AuthenticationResponse
            {
                Token = tokenHandler.WriteToken(token),
                Expiration = expiration
            };
        }

        private JwtSecurityToken CreateJwtToken(Claim[] claims, DateTime expiration, SigningCredentials sc) =>
            new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires:expiration,
                signingCredentials: sc
            );

        private Claim[] CreateClaims(IdentityUser user) =>
            new Claim[] { 
                new Claim(JwtRegisteredClaimNames.Sub,_configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

        private SigningCredentials CreateSigningCredentials() =>
            new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                SecurityAlgorithms.HmacSha256
                );
    }
}
