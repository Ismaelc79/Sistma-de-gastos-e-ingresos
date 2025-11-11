using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace Infrastructure.Test;

public class TokenServiceTests
{
    private static IConfiguration BuildConfiguration(string expireMinutes = "60")
        => new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "super-secret-key-1234567890-super-secret-key-1234567890",
                ["Jwt:Issuer"] = "tests",
                ["Jwt:Audience"] = "tests",
                ["Jwt:ExpireMinutes"] = expireMinutes
            }!)
            .Build();

    [Fact]
    public void GenerateAccessToken_ShouldEmbedUserClaims()
    {
        var service = new TokenService(BuildConfiguration());
        var user = new User(Ulid.NewUlid(), "Test", new Email("user@example.com"), Password.CreateHashed("Secret!1"));

        var token = service.GenerateAccessToken(user);

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        jwt.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Email && c.Value == "user@example.com");
    }

    [Fact]
    public void ValidateToken_WithInvalidToken_ShouldReturnFalse()
    {
        var service = new TokenService(BuildConfiguration());

        service.ValidateToken("invalid-token").Should().BeFalse();
    }

    [Fact]
    public void GetPrincipalFromExpiredToken_ShouldReturnClaimsPrincipal()
    {
        var service = new TokenService(BuildConfiguration(expireMinutes: "-1"));
        var user = new User(Ulid.NewUlid(), "Test", new Email("expired@example.com"), Password.CreateHashed("Secret!1"));
        var expiredToken = service.GenerateAccessToken(user);

        var principal = service.GetPrincipalFromExpiredToken(expiredToken);

        principal.Should().NotBeNull();
        principal!.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value.Should().Be(user.Id.ToString());
    }
}
