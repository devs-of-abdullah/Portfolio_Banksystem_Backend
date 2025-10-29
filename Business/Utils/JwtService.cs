using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;



public class JwtService 
{
    private readonly IConfiguration _configuration;
    private readonly SymmetricSecurityKey _key;
    private readonly SigningCredentials _credentials;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expireMinutes;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;

        _issuer = configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer not configured.");
        _audience = configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience not configured.");

        if (!int.TryParse(configuration["Jwt:ExpireMinutes"], out _expireMinutes))
            _expireMinutes = 120; 

        var secret = configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key not configured.");
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        _credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
    }

    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("FullName", user.FullName)
        };

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_expireMinutes),
            signingCredentials: _credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
