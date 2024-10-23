using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace JwtExample;

public class BearerTokenGenerator
{
    public string GenerateToken(string userName, string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var secret = "SimpleExampleKey1234567890123456"u8.ToArray(); //hardcoded for brevity

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity( 
            [
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Email, email)
            ]),
            
            Expires = DateTime.Now.AddMinutes(0.5),

            SigningCredentials = new SigningCredentials(
                key: new SymmetricSecurityKey(secret),
                algorithm: SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
