using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LicenseGenerator.Api.LoginModels;
using Microsoft.IdentityModel.Tokens;

namespace LicenseGenerator.Api.Services;

public class JwtService {
    private readonly IConfiguration _configuration;
    public JwtService(IConfiguration configuration) {
        _configuration = configuration;
    }
    public string GenerateToken(ApplicationUser user, IList<string> roles) {
        
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = jwtSettings["Key"]!;
        var issuer = jwtSettings["Issuer"]!;
        var audience = jwtSettings["Audience"]!;
        var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"]!);

        var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles) {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(issuer, audience, claims, 
                                         expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                                         signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}